using System.Collections.Generic;

namespace ViaJsonBuilder.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static bool HasMeaningfulValue(this string source)
        {
            return !string.IsNullOrWhiteSpace(source);
        }

        public static EnclosedKind HowEnclosed(this string source, string enclosure)
        {
            if (source.StartsWith(enclosure) && source.EndsWith(enclosure))
            {
                return EnclosedKind.Both;
            }

            if (source.StartsWith(enclosure) && !source.EndsWith(enclosure))
            {
                return EnclosedKind.Head;
            }

            if (!source.StartsWith(enclosure) && source.EndsWith(enclosure))
            {
                return EnclosedKind.Tale;
            }

            return EnclosedKind.None;
        }

        public static string Enclose(this string source, string enclosure)
        {
            var kind = source.HowEnclosed(enclosure);

            switch (kind)
            {
                case EnclosedKind.Both:
                    return source;
                case EnclosedKind.Head:
                    return $"{source}{enclosure}";
                case EnclosedKind.Tale:
                    return $"{enclosure}{source}";
                default:
                    return $"{enclosure}{source}{enclosure}";
            }
        }

        public static string Join(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string TrimStart(this string source, string trimStr)
        {
            return source.TrimStart(trimStr.ToCharArray());
        }

        public static string TrimEnd(this string source, string trimStr)
        {
            return source.TrimEnd(trimStr.ToCharArray());
        }
    }
}
