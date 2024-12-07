using System;

namespace Cart.Domain.Models
{
    public sealed class AddressModel : BaseModel<Guid>
    {
        public string Road { get; set; }
        public string Complement { get; set; }
        public string neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Number { get; set; }
    }
}
