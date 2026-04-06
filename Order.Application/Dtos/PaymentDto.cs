
using System.Text.Json.Serialization;

namespace Order.Application.Dtos
{
    public record PaymentDto
    {
        public Guid key { get; set; }
        public string txid { get; set; }
         public decimal value { get; set; }
        public DateTime? createdAt { get; set; }
        public string Qrcode { get; set; }
        public string CopyAndPaste { get; set; }
    }
}
