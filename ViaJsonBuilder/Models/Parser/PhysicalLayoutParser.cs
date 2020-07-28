using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ViaJsonBuilder.Extensions;
using ViaJsonBuilder.Models.Extractor;
using ViaJsonBuilder.Models.ProxyModels;

namespace ViaJsonBuilder.Models.Parser
{
    public class PhysicalLayoutParser
    {
        private readonly LayoutExtractor _extractor;

        public PhysicalLayoutParser()
        {
            this._extractor = new LayoutExtractor();
        }

        public PhysicalLayoutModel Parse(string raw)
        {
            var layout = this._extractor.ExtractPhysicalLayout(raw);

            if (layout.IsNullOrWhiteSpace())
            {
                return default;
            }

            return new PhysicalLayoutModel
            {
                PhysicalRows = this.Normalize(layout)
                    .Select(x => x.SplitComma())
                    .Select((x, row) =>
                    {
                        return new PhysicalRow
                        {
                            PhysicalKeys = x.Select((y, col) =>
                            {
                                return new PhysicalKey(y.Trim().ToLower())
                                {
                                    Col = col,
                                    Row = row,
                                };
                            })
                        };
                    })
            };
        }
        private IEnumerable<string> Normalize(string text)
        {
            return text.SplitNewLine()
                .Select(x => Regex.Replace(x, @"^\s*(/.*/)*\s*(.*)\s*,*\s*\\*\s*$", "$2").Trim(" ,\\\r\n"))
                .Where(x => x.HasMeaningfulValue());
        }
    }
}
