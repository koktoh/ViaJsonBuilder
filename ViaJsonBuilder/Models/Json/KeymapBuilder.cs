using System;
using System.Collections.Generic;
using System.Linq;
using Utf8Json;
using ViaJsonBuilder.Extensions;
using ViaJsonBuilder.Models.Json.KeyboardLayoutEditor;
using ViaJsonBuilder.Models.Json.QmkConfigurator;
using ViaJsonBuilder.Models.Parser;
using ViaJsonBuilder.Models.ProxyModels;

namespace ViaJsonBuilder.Models.Json
{
    public class KeymapBuilder : IJsonBuilder
    {
        private readonly PhysicalLayoutParser _physicalLayoutParser;
        private readonly LogicalLayoutParser _logicalLayoutParser;

        public KeymapBuilder()
        {
            this._physicalLayoutParser = new PhysicalLayoutParser();
            this._logicalLayoutParser = new LogicalLayoutParser();
        }

        public string Build(JsonBuildingContext context)
        {
            var qcLayout = this.GetQcLayout(context.QmkConfJson);
            var physicalLayout = this.GetPhysicalLayout(context.Raw);
            var logicalLayout = this.GetLogicalLayout(context.Raw);
            var injected = this.InjectDesign(physicalLayout, qcLayout);
            var kles = this.ConvertToKleLayout(injected, logicalLayout);
            var json = this.FormatToJson(kles);

            return json;
        }

        private QcLayout GetQcLayout(string json)
        {
            var model = JsonSerializer.Deserialize<QcJsonModel>(json);

            return model.QcLayouts.FirstOrDefault().Value ?? default;
        }

        private PhysicalLayoutModel GetPhysicalLayout(string raw)
        {
            return this._physicalLayoutParser.Parse(raw);
        }

        private LogicalLayoutModel GetLogicalLayout(string raw)
        {
            return this._logicalLayoutParser.Parse(raw);
        }

        private PhysicalLayoutModel InjectDesign(PhysicalLayoutModel physicalLayout, QcLayout qcLayout)
        {
            if (qcLayout == default)
            {
                return physicalLayout;
            }

            var counter = 0;

            return new PhysicalLayoutModel
            {
                PhysicalRows = physicalLayout.PhysicalRows
                    .Select(x =>
                    {
                        return new PhysicalRow
                        {
                            PhysicalKeys = x.PhysicalKeys
                            .Select(y =>
                            {
                                var key = qcLayout.QcKeys.ElementAtOrDefault(counter);

                                counter++;

                                return new PhysicalKey(y.Tag)
                                {
                                    Label = key.Label,
                                    Row = key.Row,
                                    Col = key.Col,
                                    Width = key.Width,
                                    Height = key.Height,
                                };
                            }),
                        };
                    }),
            };
        }

        private IEnumerable<IEnumerable<KleKey>> ConvertToKleLayout(PhysicalLayoutModel physicalLayout, LogicalLayoutModel logicalLayout)
        {
            return physicalLayout.PhysicalRows
                .Select(x =>
                {
                    return x.PhysicalKeys
                        .Select(y =>
                        {
                            var logicalKey = logicalLayout.LogicalRows
                                .SelectMany(x => x.LogicalKeys)
                                .FirstOrDefault(z => z.Tag.Equals(y.Tag));

                            var option = new KleOptionJsonModel
                            {
                                Width = y.Width,
                                Height = y.Height,
                            };

                            return new KleKey
                            {
                                LegendTopLeft = $"{logicalKey.Row},{logicalKey.Col}",
                                LegendCenterLeft = y.Label,
                                Option = option,
                            };
                        });
                });
        }

        private IEnumerable<string> ConcatCols(IEnumerable<IEnumerable<KleKey>> keys)
        {
            return keys.Select(x =>
            {
                return x.Select(y => y.ToString())
                    .JoinComma();
            });
        }

        private string ConcatRows(IEnumerable<string> rows)
        {
            return rows.Select(x => $"[{x}]")
                .Join($",{Environment.NewLine}");
        }

        private string FormatToJson(IEnumerable<IEnumerable<KleKey>> keys)
        {
            var rows = this.ConcatCols(keys);
            return this.ConcatRows(rows);
        }
    }
}
