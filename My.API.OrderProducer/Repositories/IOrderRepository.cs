using My.API.OrderProducer.Domain;
namespace My.API.OrderProducer.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order? GetById(int id);
        void Add(Order order);
    }
}
