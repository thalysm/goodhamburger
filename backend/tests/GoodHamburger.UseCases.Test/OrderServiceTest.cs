using FluentAssertions;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces.Repositories;
using GoodHamburger.Application.Services;
using GoodHamburger.CommonTestUtilities;
using GoodHamburger.Domain.Entities;
using Moq;
using Xunit;

namespace GoodHamburger.UseCases.Test;

public class OrderServiceTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMenuItemRepository> _menuItemRepositoryMock;
    private readonly OrderService _orderService;

    public OrderServiceTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _menuItemRepositoryMock = new Mock<IMenuItemRepository>();
        _orderService = new OrderService(_orderRepositoryMock.Object, _menuItemRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateOrder_WithSandwichSideAndDrink_ShouldApply20PercentDiscount()
    {
        // Arrange
        var sandwich = MenuItemBuilder.CreateSandwich();
        var side = MenuItemBuilder.CreateSide();
        var drink = MenuItemBuilder.CreateDrink();

        _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(sandwich.Id)).ReturnsAsync(sandwich);
        _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(side.Id)).ReturnsAsync(side);
        _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(drink.Id)).ReturnsAsync(drink);

        var request = new CreateOrderRequest("Customer", new List<Guid> { sandwich.Id, side.Id, drink.Id });

        // Act
        var result = await _orderService.CreateOrderAsync(request);

        // Assert
        decimal expectedSubtotal = sandwich.Price + side.Price + drink.Price;
        decimal expectedDiscount = expectedSubtotal * 0.20m;
        decimal expectedTotal = expectedSubtotal - expectedDiscount;

        result.Subtotal.Should().Be(expectedSubtotal);
        result.Discount.Should().Be(expectedDiscount);
        result.Total.Should().Be(expectedTotal);
    }

    [Fact]
    public async Task CreateOrder_WithSandwichAndDrink_ShouldApply15PercentDiscount()
    {
        // Arrange
        var sandwich = MenuItemBuilder.CreateSandwich();
        var drink = MenuItemBuilder.CreateDrink();

        _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(sandwich.Id)).ReturnsAsync(sandwich);
        _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(drink.Id)).ReturnsAsync(drink);

        var request = new CreateOrderRequest("Customer", new List<Guid> { sandwich.Id, drink.Id });

        // Act
        var result = await _orderService.CreateOrderAsync(request);

        // Assert
        decimal expectedSubtotal = sandwich.Price + drink.Price;
        decimal expectedDiscount = expectedSubtotal * 0.15m;
        decimal expectedTotal = expectedSubtotal - expectedDiscount;

        result.Subtotal.Should().Be(expectedSubtotal);
        result.Discount.Should().Be(expectedDiscount);
        result.Total.Should().Be(expectedTotal);
    }

    [Fact]
    public async Task CreateOrder_WithSandwichAndSide_ShouldApply10PercentDiscount()
    {
        // Arrange
        var sandwich = MenuItemBuilder.CreateSandwich();
        var side = MenuItemBuilder.CreateSide();

        _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(sandwich.Id)).ReturnsAsync(sandwich);
        _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(side.Id)).ReturnsAsync(side);

        var request = new CreateOrderRequest("Customer", new List<Guid> { sandwich.Id, side.Id });

        // Act
        var result = await _orderService.CreateOrderAsync(request);

        // Assert
        decimal expectedSubtotal = sandwich.Price + side.Price;
        decimal expectedDiscount = expectedSubtotal * 0.10m;
        decimal expectedTotal = expectedSubtotal - expectedDiscount;

        result.Subtotal.Should().Be(expectedSubtotal);
        result.Discount.Should().Be(expectedDiscount);
        result.Total.Should().Be(expectedTotal);
    }

    [Fact]
    public async Task CreateOrder_WithDuplicateItemType_ShouldThrowArgumentException()
    {
        // Arrange
        var sandwich1 = MenuItemBuilder.CreateSandwich("S1");
        var sandwich2 = MenuItemBuilder.CreateSandwich("S2");

        _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(sandwich1.Id)).ReturnsAsync(sandwich1);
        _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(sandwich2.Id)).ReturnsAsync(sandwich2);

        var request = new CreateOrderRequest("Customer", new List<Guid> { sandwich1.Id, sandwich2.Id });

        // Act
        var act = async () => await _orderService.CreateOrderAsync(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*não pode conter itens duplicados do tipo Sanduíche*");
    }
}
