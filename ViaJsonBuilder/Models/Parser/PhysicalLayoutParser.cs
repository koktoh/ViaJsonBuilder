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
                    .Select((row, rowIndex) =>
                    {
                        return new PhysicalRow
                        {
                            PhysicalKeys = row.Select((key, colIndex) =>
                            {
                                return new PhysicalKey(key.Trim().ToLower())
                                {
                                    Col = colIndex,
                                    Row = rowIndex,
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
