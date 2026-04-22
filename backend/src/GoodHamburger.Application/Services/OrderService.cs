using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces.Repositories;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
    Task<OrderResponse?> GetOrderByIdAsync(Guid id);
    Task<OrderResponse> UpdateOrderAsync(Guid id, UpdateOrderRequest request);
    Task DeleteOrderAsync(Guid id);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public OrderService(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository)
    {
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        var order = new Order { CustomerName = request.CustomerName };
        await PopulateOrderItems(order, request.MenuItemIds);
        order.CalculateTotals();

        await _orderRepository.AddAsync(order);

        return MapToResponse(order);
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToResponse);
    }

    public async Task<OrderResponse?> GetOrderByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order == null ? null : MapToResponse(order);
    }

    public async Task<OrderResponse> UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) throw new KeyNotFoundException("Pedido não encontrado.");

        order.CustomerName = request.CustomerName;
        order.Status = request.Status;

        var existingItemIds = order.Items.Select(i => i.MenuItemId).OrderBy(x => x).ToList();
        var newItemIds = request.MenuItemIds.OrderBy(x => x).ToList();

        if (!existingItemIds.SequenceEqual(newItemIds))
        {
            order.Items.Clear();
            await PopulateOrderItems(order, request.MenuItemIds);
            order.CalculateTotals();
        }

        await _orderRepository.UpdateAsync(order);

        return MapToResponse(order);
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        await _orderRepository.DeleteAsync(id);
    }

    private async Task PopulateOrderItems(Order order, List<Guid> menuItemIds)
    {
        var itemTypes = new HashSet<ItemType>();

        foreach (var itemId in menuItemIds)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(itemId);
            if (menuItem == null) throw new ArgumentException($"Item de cardápio com ID {itemId} não encontrado.");

            if (!itemTypes.Add(menuItem.Type))
            {
                throw new ArgumentException($"O pedido não pode conter itens duplicados do tipo {TranslateType(menuItem.Type)}.");
            }

            order.Items.Add(new OrderItem
            {
                MenuItemId = menuItem.Id,
                Name = menuItem.Name,
                Price = menuItem.Price,
                Type = menuItem.Type
            });
        }
    }

    private string TranslateType(ItemType type) => type switch
    {
        ItemType.Sandwich => "Sanduíche",
        ItemType.Side => "Acompanhamento",
        ItemType.Drink => "Refrigerante",
        _ => type.ToString()
    };

    private OrderResponse MapToResponse(Order order)
    {
        return new OrderResponse(
            order.Id,
            order.CustomerName,
            order.Status,
            order.Items.Select(i => new OrderItemResponse(i.MenuItemId, i.Name, i.Price, i.Type.ToString())).ToList(),
            order.Subtotal,
            order.Discount,
            order.Total,
            order.CreatedAt
        );
    }
}
