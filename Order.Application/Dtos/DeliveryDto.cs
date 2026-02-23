
namespace Order.Application.Dtos
{
    public record DeliveryDto
    {
        public DateOnly EstimatedDeliveryDate { get; set; }
        public int Type { get; set; }
        public decimal Price { get; set; }
    }
}
