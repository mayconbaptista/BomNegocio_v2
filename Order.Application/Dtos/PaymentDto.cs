
namespace Order.Application.Dtos
{
    public record PaymentDto
    {
        public int Type { get; set; }
        public string? CardNumber { get; set; }
        public string? CardHolderName { get; set; }
        public DateOnly? CardExpirationDate { get; set; }
        public string? CardCvv { get; set; }
    }
}
