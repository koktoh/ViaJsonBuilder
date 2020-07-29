using System.Runtime.Serialization;
using ViaJsonBuilder.Models.Json.QmkConfigurator;

namespace ViaJsonBuilder.Models.Json.KeyboardLayoutEditor
{
    public class KleOptionJsonModel
    {

        [DataMember(Name = "x")]
        public double OffsetX { get; set; }
        [DataMember(Name = "y")]
        public double OffsetY { get; set; }
        [DataMember(Name = "w")]
        public double Width { get; set; }
        [DataMember(Name = "h")]
        public double Height { get; set; }

        [IgnoreDataMember]
        public bool ShouldSerialize =>
            this.ShouldSerializeOffsetX()
            || this.ShouldSerializeOffsetY()
            || this.ShouldSerializeWidth()
            || this.ShouldSerializeHeight();

        public bool ShouldSerializeOffsetX() => this.OffsetX != 0;
        public bool ShouldSerializeOffsetY() => this.OffsetY != 0;
        public bool ShouldSerializeWidth() => this.Width != 0;
        public bool ShouldSerializeHeight() => this.Height != 0;

        public KleOptionJsonModel() { }

        public KleOptionJsonModel(QcKey qcKey)
        {
            this.Width = qcKey.Width;
            this.Height = qcKey.Height;
        }
    }
}
