using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public abstract class EntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

public class MenuItem : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ItemType Type { get; set; }
    public string? ImageUrl { get; set; }
}

public class Order : EntityBase
{
    public string CustomerName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public List<OrderItem> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void CalculateTotals()
    {
        Subtotal = Items.Sum(i => i.Price);
        
        bool hasSandwich = Items.Any(i => i.Type == ItemType.Sandwich);
        bool hasSide = Items.Any(i => i.Type == ItemType.Side);
        bool hasDrink = Items.Any(i => i.Type == ItemType.Drink);

        decimal discountPercentage = 0;

        if (hasSandwich && hasSide && hasDrink)
        {
            discountPercentage = 0.20m;
        }
        else if (hasSandwich && hasDrink)
        {
            discountPercentage = 0.15m;
        }
        else if (hasSandwich && hasSide)
        {
            discountPercentage = 0.10m;
        }

        Discount = Subtotal * discountPercentage;
        Total = Subtotal - Discount;
    }
}

public class OrderItem : EntityBase
{
    public Guid MenuItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ItemType Type { get; set; }
}
