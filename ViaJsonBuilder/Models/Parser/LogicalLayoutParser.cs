using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ViaJsonBuilder.Extensions;
using ViaJsonBuilder.Models.Extractor;
using ViaJsonBuilder.Models.ProxyModels;

namespace ViaJsonBuilder.Models.Parser
{
    public class LogicalLayoutParser
    {
        private readonly LayoutExtractor _extractor;

        public LogicalLayoutParser()
        {
            this._extractor = new LayoutExtractor();
        }

        public LogicalLayoutModel Parse(string raw)
        {
            var layout = this._extractor.ExtractLogicalLayout(raw);

            if (layout.IsNullOrWhiteSpace())
            {
                return default;
            }

            return new LogicalLayoutModel
            {
                LogicalRows = this.Normalize(layout)
                    .Select(x => x.SplitComma())
                    .Select((row, rowIndex) =>
                    {
                        return new LogicalRow
                        {
                            LogicalKeys = row.Select((key, colIndex) =>
                            {
                                return new LogicalKey(key.Trim().ToLower())
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
                .Select(x => Regex.Replace(x, @"^.*\{\s*(.*)\s*,*\s*\}.*$", "$1"))
                .Where(x => x.HasMeaningfulValue());
        }
    }
}
