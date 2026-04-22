using Bogus;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.CommonTestUtilities;

public static class MenuItemBuilder
{
    public static MenuItem CreateSandwich(string name = "Sandwich", decimal price = 5.0m) =>
        new MenuItem { Id = Guid.NewGuid(), Name = name, Price = price, Type = ItemType.Sandwich };

    public static MenuItem CreateSide(string name = "Side", decimal price = 2.0m) =>
        new MenuItem { Id = Guid.NewGuid(), Name = name, Price = price, Type = ItemType.Side };

    public static MenuItem CreateDrink(string name = "Drink", decimal price = 2.5m) =>
        new MenuItem { Id = Guid.NewGuid(), Name = name, Price = price, Type = ItemType.Drink };
}
