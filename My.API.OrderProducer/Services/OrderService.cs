using My.API.OrderProducer.Domain;
using My.API.OrderProducer.Repositories;
namespace My.API.OrderProducer.Services;
public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo)
    {
        _repo = repo;
    }
    public IEnumerable<Order> GetOrders() => _repo.GetAll();
    public Order? GetOrder(int id) => _repo.GetById(id);
    public Order CreateOrder(Order order)
    {
        _repo.Add(order);
        return order;
    }
}