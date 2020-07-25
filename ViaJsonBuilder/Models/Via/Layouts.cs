using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ViaJsonBuilder.Models.Via
{
    public class Layouts
    {
        [DataMember(Name = "labels")]
        public IEnumerable<dynamic> Labels { get; set; }
        [DataMember(Name = "keymap")]
        public dynamic Keymap { get; set; }

        public bool ShouldSerializeLabels() => this.Labels?.Any() ?? false;
    }
}
