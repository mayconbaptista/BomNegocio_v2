using Order.Domain.Entities;

namespace Order.Domain.Interfaces
{
    public interface IOrderRepository : IWriteRepository<OrderEntity>
    {

    }
}
