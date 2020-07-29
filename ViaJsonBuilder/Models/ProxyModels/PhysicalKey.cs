using ViaJsonBuilder.Models.Json.QmkConfigurator;

namespace ViaJsonBuilder.Models.ProxyModels
{
    public class PhysicalKey
    {
        public string Tag { get; }
        public string Label { get; set; }
        public double Col { get; set; }
        public double Row { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public PhysicalKey(string tag)
        {
            this.Tag = tag;
        }

        public PhysicalKey(string tag, QcKey qcKey)
        {
            this.Tag = tag;
            this.Label = qcKey.Label;
            this.Col = qcKey.X;
            this.Row = qcKey.Y;
            this.Width = qcKey.Width;
            this.Height = qcKey.Height;
        }
    }
}
