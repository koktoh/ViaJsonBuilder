using System.Runtime.Serialization;

namespace ViaJsonBuilder.Models.Via
{
    public class ViaModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "vendorId")]
        public string VenderId { get; set; }
        [DataMember(Name = "productId")]
        public string ProductId { get; set; }
        [DataMember(Name = "lighting")]
        public Lighting Lighting { get; set; }
        [DataMember(Name = "matrix")]
        public Matrix Matrix { get; set; }
        [DataMember(Name = "layouts")]
        public Layouts Layouts { get; set; }
    }
}
