namespace Order.Domain.Abstractions
{
    public interface IOrderUnitOfWork
    {
        public IOrderRepository Orders { get; }
    }
}
