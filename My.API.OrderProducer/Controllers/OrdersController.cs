using Microsoft.AspNetCore.Mvc;
using My.API.OrderProducer.Domain;
using My.API.OrderProducer.Kafka;
using My.API.OrderProducer.Services;
namespace My.API.OrderProducer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMessageProducer _producer;
    private readonly ILogger<OrdersController> _logger;
    public OrdersController(
            IOrderService orderService,
            IMessageProducer producer,
            ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _producer = producer;
        _logger = logger;
    }
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "ok" });
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var orders = _orderService.GetOrders();
        return Ok(orders);
    }
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var order = _orderService.GetOrder(id);
        if (order is null)
        {
            return NotFound();
        }
        return Ok(order);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var created = _orderService.CreateOrder(order);
        var message = $"New order created: {created.Id}, Customer={created.Customer}, Amount={created.Amount}";
        await _producer.ProduceAsync(topic: string.Empty, message: message);
        _logger.LogInformation("Order created and Kafka event published: {Id}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}

