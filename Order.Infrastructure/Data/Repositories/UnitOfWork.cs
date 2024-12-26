
using Order.Domain.Interfaces;

namespace Order.Infrastructure.Data.Repositories
{
    internal sealed class UnitOfWork(OrderContext orderContext) : IUnitOfWork
    {
        public readonly OrderContext _context = orderContext;

        private IOrderRepository? _orderRepository;

        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_context);
                }
                return _orderRepository;
            }
        }

        public int Commit()
        {
            return this._context.SaveChanges();
        }

        public async ValueTask<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await this._context.SaveChangesAsync(cancellationToken);
        }
    }
}
