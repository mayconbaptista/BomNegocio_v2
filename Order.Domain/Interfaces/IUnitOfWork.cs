namespace Order.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public IOrderRepository OrderRepository { get; }

        public int Commit();
        public ValueTask<int> CommitAsync(CancellationToken cancellationToken);
    }
}
