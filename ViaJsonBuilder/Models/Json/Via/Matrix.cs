using System.Runtime.Serialization;

namespace ViaJsonBuilder.Models.Json.Via
{
    public class Matrix
    {
        [DataMember(Name = "rows")]
        public int Rows { get; set; }
        [DataMember(Name = "cols")]
        public int Cols { get; set; }
    }
}
