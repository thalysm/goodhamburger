using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.Infrastructure;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new GoodHamburgerContext(
            serviceProvider.GetRequiredService<DbContextOptions<GoodHamburgerContext>>());

        await context.Database.EnsureCreatedAsync();

        if (context.MenuItems.Any())
        {
            return; // DB has been seeded
        }

        var menuItems = new MenuItem[]
        {
            new MenuItem { Name = "X Burger", Price = 5.00m, Type = ItemType.Sandwich, ImageUrl = "/images/x_burger.png" },
            new MenuItem { Name = "X Egg", Price = 4.50m, Type = ItemType.Sandwich, ImageUrl = "/images/x_egg.png" },
            new MenuItem { Name = "X Bacon", Price = 7.00m, Type = ItemType.Sandwich, ImageUrl = "/images/x_bacon.png" },
            new MenuItem { Name = "Batata frita", Price = 2.00m, Type = ItemType.Side, ImageUrl = "/images/french_fries.png" },
            new MenuItem { Name = "Refrigerante", Price = 2.50m, Type = ItemType.Drink, ImageUrl = "/images/soda_drink.png" }
        };

        foreach (var item in menuItems)
        {
            context.MenuItems.Add(item);
        }

        await context.SaveChangesAsync();
    }
}
