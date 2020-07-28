using System.Runtime.Serialization;
using ViaJsonBuilder.Models.Json.QmkConfigurator;

namespace ViaJsonBuilder.Models.Json.KeyboardLayoutEditor
{
    public class KleOptionJsonModel
    {
        [DataMember(Name = "w")]
        public double Width { get; set; }
        [DataMember(Name = "h")]
        public double Height { get; set; }

        [IgnoreDataMember]
        public bool ShouldSerialize =>
            this.ShouldSerializeWidth()
            || this.ShouldSerializeHeight();

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
