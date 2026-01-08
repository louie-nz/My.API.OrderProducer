using My.API.OrderProducer.Domain;
namespace My.API.OrderProducer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        //demo 
        private readonly List<Order> _orders = new()
        {
            new Order { Id = 1, Customer = "Alice", Amount = 120.50m, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new Order { Id = 2, Customer = "Bob",   Amount = 300.00m, CreatedAt = DateTime.UtcNow.AddDays(-1) }
        };
        public IEnumerable<Order> GetAll() => _orders;
        public Order? GetById(int id) => _orders.FirstOrDefault(o => o.Id == id);
        public void Add(Order order)
        {
            var nextId = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;
            order.Id = nextId;
            order.CreatedAt = DateTime.UtcNow;
            _orders.Add(order);
        }
    }
}
