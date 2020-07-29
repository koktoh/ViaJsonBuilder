using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utf8Json;
using ViaJsonBuilder.Extensions;

namespace ViaJsonBuilder.Models.Json.KeyboardLayoutEditor
{
    public class KleKey
    {
        public string LegendTopLeft { get; set; }
        public string LegendTopCenter { get; set; }
        public string LegendTopRight { get; set; }
        public string LegendCenterLeft { get; set; }
        public string LegendCenter { get; set; }
        public string LegendCenterRight { get; set; }
        public string LegendBottomLeft { get; set; }
        public string LegendBottomCenter { get; set; }
        public string LegendBottomRight { get; set; }
        public string LegendFrontLeft { get; set; }
        public string LegendFrontCenter { get; set; }
        public string LegendFrontRight { get; set; }

        public string Legend => this.BuildLegend();

        public KleOptionJsonModel Option { get; set; }

        private string BuildLegend()
        {
            var regend = this.EnumerateLegendParts().Join(@"\n");
            return Regex.Replace(regend, @"^(.*?)(\\n)*$", "$1");
        }

        private IEnumerable<string> EnumerateLegendParts()
        {
            yield return this.LegendTopLeft.Escape();
            yield return this.LegendBottomLeft.Escape();
            yield return this.LegendTopRight.Escape();
            yield return this.LegendBottomRight.Escape();
            yield return this.LegendFrontLeft.Escape();
            yield return this.LegendFrontRight.Escape();
            yield return this.LegendCenterLeft.Escape();
            yield return this.LegendCenterRight.Escape();
            yield return this.LegendTopCenter.Escape();
            yield return this.LegendCenter.Escape();
            yield return this.LegendBottomCenter.Escape();
            yield return this.LegendFrontCenter.Escape();
        }

        public override string ToString()
        {
            var option = JsonSerializer.ToJsonString(this.Option);

            return $@"{(this.Option.ShouldSerialize ? $"{option}," : string.Empty)}""{this.Legend}""";
        }
    }
}
