using System.Runtime.Serialization;

namespace ViaJsonBuilder.Models.Json.QmkConfigurator
{
    public class QcKey
    {
        [DataMember(Name = "label")]
        public string Label { get; set; }
        [DataMember(Name = "x")]
        public double Col { get; set; }
        [DataMember(Name = "y")]
        public double Row { get; set; }
        [DataMember(Name = "w")]
        public double Width { get; set; }
        [DataMember(Name = "h")]
        public double Height { get; set; }
    }
}
