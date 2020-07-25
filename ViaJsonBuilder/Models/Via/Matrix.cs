using System.Runtime.Serialization;

namespace ViaJsonBuilder.Models.Via
{
    public class Matrix
    {
        [DataMember(Name = "rows")]
        public int Rows { get; set; }
        [DataMember(Name = "cols")]
        public int Cols { get; set; }
    }
}
