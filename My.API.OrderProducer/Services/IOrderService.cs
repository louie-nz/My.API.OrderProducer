using My.API.OrderProducer.Domain;
namespace My.API.OrderProducer.Services;
public interface IOrderService
{
    IEnumerable<Order> GetOrders();
    Order? GetOrder(int id);
    Order CreateOrder(Order order);
}
