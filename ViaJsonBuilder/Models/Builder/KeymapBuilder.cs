using System;
using System.Collections.Generic;
using System.Linq;
using Utf8Json;
using ViaJsonBuilder.Extensions;
using ViaJsonBuilder.Models.Json.KeyboardLayoutEditor;
using ViaJsonBuilder.Models.Json.QmkConfigurator;
using ViaJsonBuilder.Models.Parser;
using ViaJsonBuilder.Models.ProxyModels;

namespace ViaJsonBuilder.Models.Builder
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
            var kleContext = context.KleContext;

            var qcLayout = this.GetQcLayout(kleContext.QmkConfJson);
            var physicalLayout = this.GetPhysicalLayout(kleContext.LayoutDefinition);
            var logicalLayout = this.GetLogicalLayout(kleContext.LayoutDefinition);
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

            double originY = 0;
            var counter = 0;

            return new PhysicalLayoutModel
            {
                PhysicalRows = physicalLayout.PhysicalRows
                    .Select((row, rowIndex) =>
                    {
                        double offsetX = 0;
                        double offsetY = 0;

                        return new PhysicalRow
                        {
                            PhysicalKeys = row.PhysicalKeys
                            .Select((key, colIndex) =>
                            {
                                var qcKey = qcLayout.QcKeys.ElementAtOrDefault(counter);

                                if (rowIndex == 0 && colIndex == 0)
                                {
                                    originY = qcKey.Y;
                                    offsetY = qcKey.Y;
                                }
                                else if (colIndex == 0)
                                {
                                    offsetY = qcKey.Y - originY - rowIndex;
                                }
                                else
                                {
                                    var preQcKey = qcLayout.QcKeys.ElementAtOrDefault(counter - 1);
                                    offsetY = qcKey.Y - preQcKey.Y;
                                }

                                counter++;

                                var physKey = new PhysicalKey(key.Tag)
                                {
                                    Label = qcKey.Label,
                                    Row = rowIndex,
                                    Col = colIndex,
                                    OffsetX = Math.Round(qcKey.X - offsetX - colIndex, 3),
                                    OffsetY = Math.Round(offsetY, 3),
                                    Width = qcKey.Width,
                                    Height = qcKey.Height,
                                };

                                if (physKey.Width != 0)
                                {
                                    offsetX += physKey.Width - 1;
                                }

                                if (physKey.OffsetX != 0)
                                {
                                    offsetX += physKey.OffsetX;
                                }

                                return physKey;
                            }),
                        };
                    }),
            };
        }

        private IEnumerable<IEnumerable<KleKey>> ConvertToKleLayout(PhysicalLayoutModel physicalLayout, LogicalLayoutModel logicalLayout)
        {
            return physicalLayout.PhysicalRows
                .Select(row =>
                {
                    return row.PhysicalKeys
                        .Select(key =>
                        {
                            var logicalKey = logicalLayout.LogicalRows
                                .SelectMany(x => x.LogicalKeys)
                                .FirstOrDefault(x => x.Tag.Equals(key.Tag));

                            var option = new KleOptionJsonModel
                            {
                                OffsetX = key.OffsetX,
                                OffsetY = key.OffsetY,
                                Width = key.Width,
                                Height = key.Height,
                            };

                            return new KleKey
                            {
                                LegendTopLeft = $"{logicalKey.Row},{logicalKey.Col}",
                                LegendCenterLeft = key.Label,
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
