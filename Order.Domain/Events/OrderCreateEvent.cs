using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Events
{
    public sealed class OrderCreateEvent : BaseEvent
    {
        public List<Product> Products { get; private set; }

        public OrderCreateEvent(List<Product> products)
        {
            Products = products;
        }
    }
}
