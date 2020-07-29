using System.Runtime.Serialization;

namespace ViaJsonBuilder.Models.Json.QmkConfigurator
{
    public class QcKey
    {
        [DataMember(Name = "label")]
        public string Label { get; set; }
        [DataMember(Name = "x")]
        public double X { get; set; }
        [DataMember(Name = "y")]
        public double Y { get; set; }
        [DataMember(Name = "w")]
        public double Width { get; set; }
        [DataMember(Name = "h")]
        public double Height { get; set; }
    }
}
