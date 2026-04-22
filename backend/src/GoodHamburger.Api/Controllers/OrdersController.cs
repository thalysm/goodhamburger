using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(CreateOrderRequest request)
    {
        try
        {
            var result = await _orderService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll()
    {
        return Ok(await _orderService.GetAllOrdersAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetById(Guid id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        if (result == null) return NotFound(new { message = "Pedido não encontrado." });
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderResponse>> Update(Guid id, UpdateOrderRequest request)
    {
        try
        {
            var result = await _orderService.UpdateOrderAsync(id, request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _orderService.DeleteOrderAsync(id);
        return NoContent();
    }
}
