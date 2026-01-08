using Xunit;
using My.API.OrderProducer.Services;

namespace My.API.OrderProducer.Tests;

public class OrderServiceTests
{
 [Fact]
 public void CreateOrder_ShouldReturnOrderId()
 {
 // Arrange: create repository and service
 var repo = new My.API.OrderProducer.Repositories.OrderRepository();
 var service = new OrderService(repo);

 // Act: create an order
 var order = new My.API.OrderProducer.Domain.Order { Customer = "test", Amount =10.0m };
 var created = service.CreateOrder(order);

 // Assert
 Assert.NotNull(created);
 Assert.True(created.Id >0);
 }
}