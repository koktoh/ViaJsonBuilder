using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ViaJsonBuilder.Extensions;

namespace ViaJsonBuilder.Models.Json
{
    public class KeymapBuilder : IJsonBuilder
    {
        private const string LAYOUT = "layout";

        private const double REX_TIMEOUT = 5;

        private readonly Regex _layoutRex;
        private readonly Regex _physicalLayoutRex;

        public KeymapBuilder()
        {
            this._layoutRex = new Regex($@"(\r|\n)(?<{LAYOUT}>(\#define LAYOUT)(.*(\r|\n))+?\s*\}})", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
            this._physicalLayoutRex = new Regex($@".*\(.*\\\s*(\r|\n)*(?<{LAYOUT}>(.*(\r|\n))+?.*)\s*\)", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
        }

        public string Build(JsonBuildingContext context)
        {
            var layoutBlock = this.ExtractLayoutBlock(context.Raw);

            if (layoutBlock.IsNullOrWhiteSpace())
            {
                return default;
            }

            var definitions = this.ExtractLayoutDefinitions(layoutBlock);

            if (!definitions?.Any() ?? false)
            {
                return default;
            }

            var physicalLayout = this.ExtractPhysicalLayoutBlock(layoutBlock);

            var pair = this.ConvertToDefinitionKlePair(definitions);

            var kle = this.GetPhysicalKle(physicalLayout, pair);

            var json = this.ConvertToJson(kle);

            return json;
        }

        private string ExtractPhysicalLayoutBlock(string block)
        {
            var matched = this._physicalLayoutRex.Match(block).Groups[LAYOUT];

            if (matched.Success)
            {
                return matched.Value
                    .Replace("\\", string.Empty)
                    .ToLower()
                    .Trim();
            }

            return default;
        }

        private string ExtractLayoutBlock(string raw)
        {
            var matched = this._layoutRex.Match(raw).Groups[LAYOUT];

            if (matched.Success)
            {
                return matched.Value;
            }

            return default;
        }

        private IEnumerable<string> ExtractLayoutDefinitions(string block)
        {
            return block.Split(Environment.NewLine)
                .Where(x => Regex.IsMatch(x, @"^\s*\{.*?\}", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT)))
                .Select(x => Regex.Replace(x, @"(\s*\{)(.*)(\},?\s*\\)", "$2", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT)));
        }

        private IDictionary<string, string> ConvertToDefinitionKlePair(IEnumerable<string> definitions)
        {
            return definitions
                .SelectMany((x, row) =>
                {
                    return x.Split(",")
                    .Select(y => y.Trim())
                    .Select((y, col) =>
                    {
                        var isKcNo = y.ToUpper().Equals("KC_NO") || y.Equals("XXXXXXX");

                        return new { Key = y.ToLower(), Value = $@"{(isKcNo ? @"{""d"":true}," : string.Empty)}""{row},{col}""" };
                    });
                })
                .Where(x => !x.Key.Equals("kc_no"))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private IEnumerable<string> GetPhysicalKle(string physicalLayout, IDictionary<string, string> dict)
        {
            foreach (var elem in dict)
            {
                physicalLayout = physicalLayout.Replace(elem.Key, elem.Value);
            }

            return physicalLayout.Split(Environment.NewLine)
                .Select(x => x.TrimEnd().TrimEnd(","));
        }

        private string ConvertToJson(IEnumerable<string> kle)
        {
            return kle.Select(x => $"[{x}]").Join($",{Environment.NewLine}");
        }
    }
}
