using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO;
using WebStore.Interfaces.Services;

namespace WebStore.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersApiController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersApiController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("user/{userName}")]
    public async Task<IActionResult> GetUserOrders(string userName, CancellationToken cancellationToken = default)
    {
        var result = await _orderService.GetUserOrdersAsync(userName, cancellationToken);
        return Ok(result.ToDto());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _orderService.GetOrderByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound();
        return Ok(result.ToDto());
    }

    [HttpPost("{userName}")]
    public async Task<IActionResult> CreateNewOrderAsync(string userName, [FromBody] CreateOrderDto? model)
    {
        var result = await _orderService.CreateNewOrderAsync(userName, model!.Items!.ToCartView(), model!.Order!);
        return CreatedAtAction(nameof(GetOrderById), new {result.Id}, result.ToDto());
    }
}