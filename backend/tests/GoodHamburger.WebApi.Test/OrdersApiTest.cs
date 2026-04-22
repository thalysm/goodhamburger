using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GoodHamburger.WebApi.Test;

public class OrdersApiTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public OrdersApiTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetMenu_ShouldReturnMenuItems()
    {
        // Act
        var response = await _client.GetAsync("api/menu");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<MenuItemResponse>>();
        items.Should().NotBeNull();
        items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateOrder_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var menuResponse = await _client.GetFromJsonAsync<List<MenuItemResponse>>("api/menu");
        var sandwichId = menuResponse!.First(i => i.Type == "Sandwich").Id;
        var drinkId = menuResponse!.First(i => i.Type == "Drink").Id;

        var request = new CreateOrderRequest("Integration Test", new List<Guid> { sandwichId, drinkId });

        // Act
        var response = await _client.PostAsJsonAsync("api/orders", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
        result.Should().NotBeNull();
        result!.CustomerName.Should().Be("Integration Test");
        result.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateOrder_WithDuplicateType_ShouldReturnBadRequest()
    {
        // Arrange
        var menuResponse = await _client.GetFromJsonAsync<List<MenuItemResponse>>("api/menu");
        var sandwich1Id = menuResponse!.First(i => i.Type == "Sandwich").Id;
        var sandwiches = menuResponse!.Where(i => i.Type == "Sandwich").ToList();
        
        if (sandwiches.Count < 2) return; 

        var request = new CreateOrderRequest("Integration Test", new List<Guid> { sandwiches[0].Id, sandwiches[1].Id });

        // Act
        var response = await _client.PostAsJsonAsync("api/orders", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
