
using Order.Domain.Entities;
using Order.Domain.Interfaces;

namespace Order.Infrastructure.Data.Repositories
{
    internal class OrderRepository(OrderContext orderContext) 
        : WriteRepository<OrderEntity>(orderContext), IOrderRepository
    {

    }
}
