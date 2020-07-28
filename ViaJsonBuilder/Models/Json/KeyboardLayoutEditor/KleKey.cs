using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utf8Json;
using ViaJsonBuilder.Extensions;

namespace ViaJsonBuilder.Models.Json.KeyboardLayoutEditor
{
    public class KleKey
    {
        public string RegendTopLeft { get; set; }
        public string RegendTopCenter { get; set; }
        public string RegendTopRight { get; set; }
        public string RegendCenterLeft { get; set; }
        public string RegendCenter { get; set; }
        public string RegendCenterRight { get; set; }
        public string RegendBottomLeft { get; set; }
        public string RegendBottomCenter { get; set; }
        public string RegendBottomRight { get; set; }
        public string RegendFrontLeft { get; set; }
        public string RegendFrontCenter { get; set; }
        public string RegendFrontRight { get; set; }

        public string Regend => this.BuildRegend();

        public KleOptionJsonModel Option { get; set; }

        private string BuildRegend()
        {
            var regend = this.EnumerateRegendParts().Join(@"\n");
            return Regex.Replace(regend, @"^(.*?)(\\n)*$", "$1");
        }

        private IEnumerable<string> EnumerateRegendParts()
        {
            yield return this.RegendTopLeft.Escape();
            yield return this.RegendBottomLeft.Escape();
            yield return this.RegendTopRight.Escape();
            yield return this.RegendBottomRight.Escape();
            yield return this.RegendFrontLeft.Escape();
            yield return this.RegendFrontRight.Escape();
            yield return this.RegendCenterLeft.Escape();
            yield return this.RegendCenterRight.Escape();
            yield return this.RegendTopCenter.Escape();
            yield return this.RegendCenter.Escape();
            yield return this.RegendBottomCenter.Escape();
            yield return this.RegendFrontCenter.Escape();
        }

        public override string ToString()
        {
            var option = JsonSerializer.ToJsonString(this.Option);

            return $@"{(this.Option.ShouldSerialize ? $"{option}," : string.Empty)}""{this.Regend}""";
        }
    }
}
