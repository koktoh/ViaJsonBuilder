using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utf8Json;
using ViaJsonBuilder.Extensions;
using ViaJsonBuilder.Models.Json.Via;

namespace ViaJsonBuilder.Models.Builder
{
    public class ViaBuilder : IJsonBuilder
    {
        public string Build(JsonBuildingContext context)
        {
            var viaContext = context.ViaContext;

            var model = this.GetViaModel(viaContext);

            return JsonSerializer.ToJsonString(model);
        }

        private ViaModel GetViaModel(ViaContext context)
        {
            var name = context.Name;
            var vendorId = context.VendorId;
            var productId = context.ProductId;

            return new ViaModel
            {
                Name = name,
                VenderId = vendorId,
                ProductId = productId,
                Lighting = this.GetLighting(context),
                Matrix = this.GetMatrix(context),
                Layouts = this.GetLayouts(context),
            };
        }

        private Lighting GetLighting(ViaContext context)
        {
            if (Enum.TryParse<Lighting>(context.Lighting, out var lighting))
            {
                return lighting;
            }

            return Lighting.none;
        }

        private Layouts GetLayouts(ViaContext context)
        {
            return new Layouts
            {
                Labels = this.GetLabels(context),
                Keymap = this.GetKeymap(context),
            };
        }

        private IEnumerable<object> GetLabels(ViaContext context)
        {
            var labels = context.Labels;

            if (labels.IsNullOrWhiteSpace())
            {
                return default;
            }

            var formatted = labels.Split(",")
                .Select(x =>
                {
                    if (x.StartsWith("[") && !x.EndsWith("]"))
                    {
                        return "[" + x.TrimStart('[').Enclose(@"""");
                    }

                    if (!x.StartsWith("[") && x.EndsWith("]"))
                    {
                        return x.TrimEnd(']').Enclose(@"""") + "]";
                    }

                    return x.Enclose(@"""");
                })
                .JoinComma();

            if (!formatted.StartsWith("["))
            {
                formatted = $"[{formatted}]";
            }

            return JsonSerializer.Deserialize<IEnumerable<object>>(formatted);
        }

        private object GetKeymap(ViaContext context)
        {
            var keymap = context.Keymap;

            if (keymap.IsNullOrWhiteSpace())
            {
                return default;
            }

            if(Regex.IsMatch(keymap, @"^\s*\[\s*\[(.+,?\s*)*\]\s*\]\s*$"))
            {
                return JsonSerializer.Deserialize<object>(keymap);
            }

            return JsonSerializer.Deserialize<object>($"[{keymap}]");
        }

        private Matrix GetMatrix(ViaContext context)
        {
            var matrix = new Matrix();

            if (int.TryParse(context.Rows, out var rows))
            {
                matrix.Rows = rows;
            }

            if (int.TryParse(context.Cols, out var cols))
            {
                matrix.Cols = cols;
            }

            return matrix;
        }
    }
}
