using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ViaJsonBuilder.Models.Json.QmkConfigurator
{
    public class QcJsonModel
    {
        [DataMember(Name = "keyboard_name")]
        public string KeyboardName { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "maintainer")]
        public string Maintainer { get; set; }
        [DataMember(Name = "width")]
        public double Width { get; set; }
        [DataMember(Name = "height")]
        public double Height { get; set; }
        [DataMember(Name = "layouts")]
        public IDictionary<string, QcLayout> QcLayouts { get; set; }
    }
}
