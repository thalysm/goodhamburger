using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.DTOs;

public record MenuItemResponse(Guid Id, string Name, decimal Price, string Type, string? ImageUrl);

public record CreateOrderRequest(string CustomerName, List<Guid> MenuItemIds);

public record UpdateOrderRequest(string CustomerName, OrderStatus Status, List<Guid> MenuItemIds);

public record OrderItemResponse(Guid MenuItemId, string Name, decimal Price, string Type);

public record OrderResponse(
    Guid Id,
    string CustomerName,
    OrderStatus Status,
    List<OrderItemResponse> Items,
    decimal Subtotal,
    decimal Discount,
    decimal Total,
    DateTime CreatedAt
);
