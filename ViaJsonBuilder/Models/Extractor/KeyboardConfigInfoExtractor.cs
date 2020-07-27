using System;
using System.Text.RegularExpressions;

namespace ViaJsonBuilder.Models.Extractor
{
    public class KeyboardConfigInfoExtractor
    {
        private const string NAME = "name";
        private const string VENDOR_ID = "vendor";
        private const string PRODUCT_ID = "product";
        private const string ROWS = "rows";
        private const string COLS = "cols";

        private const double REX_TIMEOUT = 3;

        private readonly Regex _nameRex;
        private readonly Regex _vendorRex;
        private readonly Regex _productRex;
        private readonly Regex _rowsRex;
        private readonly Regex _colsRex;

        public KeyboardConfigInfoExtractor()
        {
            this._nameRex = new Regex($@".*(\r|\n).*?PRODUCT\s+(?<{NAME}>.*?)(\r|\n).*", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
            this._vendorRex = new Regex($@".*(\r|\n).*?VENDOR_ID\s+(?<{VENDOR_ID}>.*)(\r|\n).*", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
            this._productRex = new Regex($@".*(\r|\n).*?PRODUCT_ID\s+(?<{PRODUCT_ID}>.*)(\r|\n).*", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
            this._rowsRex = new Regex($@"(\r|\n)\s*(\#define\s+MATRIX_ROWS\s+)(?<{ROWS}>\d+)", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
            this._colsRex = new Regex($@"(\r|\n)\s*(\#define\s+MATRIX_COLS\s+)(?<{COLS}>\d+)", RegexOptions.None, TimeSpan.FromSeconds(REX_TIMEOUT));
        }

        public KeyboardConfigInfo Extract(string text)
        {
            return new KeyboardConfigInfo
            {
                Name = this.ExtractName(text),
                VendorId = this.ExtractVendorId(text),
                ProductId = this.ExtractProductId(text),
                Rows = this.ExtractRows(text),
                Cols = this.ExtractCols(text),
            };
        }

        private string ExtractName(string text)
        {
            var matched = this._nameRex.Match(text).Groups[NAME];

            if (matched.Success)
            {
                return matched.Value.Trim();
            }

            return default;
        }

        private string ExtractVendorId(string text)
        {
            var matched = this._vendorRex.Match(text).Groups[VENDOR_ID];

            if (matched.Success)
            {
                return matched.Value.Trim();
            }

            return default;
        }

        private string ExtractProductId(string text)
        {
            var matched = this._productRex.Match(text).Groups[PRODUCT_ID];

            if (matched.Success)
            {
                return matched.Value.Trim();
            }

            return default;
        }

        private string ExtractRows(string text)
        {
            var matched = this._rowsRex.Match(text).Groups[ROWS];

            if (matched.Success)
            {
                return matched.Value.Trim();
            }

            return default;
        }

        private string ExtractCols(string text)
        {
            var matched = this._colsRex.Match(text).Groups[COLS];

            if (matched.Success)
            {
                return matched.Value.Trim();
            }

            return default;
        }
    }
}
