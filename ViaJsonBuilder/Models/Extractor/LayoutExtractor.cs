using System;
using System.Text.RegularExpressions;
using ViaJsonBuilder.Extensions;

namespace ViaJsonBuilder.Models.Extractor
{
    public class LayoutExtractor
    {
        private const string LAYOUT = "layout";

        private const double REX_TIMEOUT = 5;

        private readonly Regex _layoutRex;
        private readonly Regex _physicalLayoutRex;
        private readonly Regex _logicalLayoutRex;

        public LayoutExtractor()
        {
            this._layoutRex = new Regex($@"(\r|\n)(?<{LAYOUT}>(\#define LAYOUT)(.*(\r|\n))+?\s*\}})", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
            this._physicalLayoutRex = new Regex($@".*\(.*\\\s*(\r|\n)*(?<{LAYOUT}>(.*(\r|\n))+?.*)\s*\)", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
            this._logicalLayoutRex = new Regex($@"\{{.*(\r|\n)*(?<{LAYOUT}>(.*(\r|\n))+).*\}}", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
        }

        public string ExtractPhysicalLayout(string raw)
        {
            var layout = this.ExtractLayout(raw);

            if (layout.IsNullOrWhiteSpace())
            {
                return default;
            }

            try
            {
                var matched = this._physicalLayoutRex.Match(layout).Groups[LAYOUT];

                if (matched.Success)
                {
                    return matched.Value;
                }

                return default;
            }
            catch
            {
                return default;
            }
        }

        public string ExtractLogicalLayout(string raw)
        {
            var layout = this.ExtractLayout(raw);

            if (layout.IsNullOrWhiteSpace())
            {
                return default;
            }

            try
            {
                var matched = this._logicalLayoutRex.Match(layout).Groups[LAYOUT];

                if (matched.Success)
                {
                    return matched.Value;
                }

                return default;
            }
            catch
            {
                return default;
            }

        }

        private string ExtractLayout(string raw)
        {
            try
            {
                var matched = this._layoutRex.Match(raw).Groups[LAYOUT];

                if (matched.Success)
                {
                    return matched.Value;
                }

                return default;
            }
            catch
            {
                return default;
            }
        }
    }
}
