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
                    .Select((x, row) =>
                    {
                        return new LogicalRow
                        {
                            LogicalKeys = x.Select((y, col) =>
                            {
                                return new LogicalKey(y.Trim().ToLower())
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
                .Select(x => Regex.Replace(x, @"^.*\{\s*(.*)\s*,*\s*\}.*$", "$1"))
                .Where(x => x.HasMeaningfulValue());
        }
    }
}
