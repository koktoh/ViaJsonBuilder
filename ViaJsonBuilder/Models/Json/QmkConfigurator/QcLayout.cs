using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ViaJsonBuilder.Models.Json.QmkConfigurator
{
    public class QcLayout
    {
        [DataMember(Name = "layout")]
        public IEnumerable<QcKey> QcKeys { get; set; }
    }
}
