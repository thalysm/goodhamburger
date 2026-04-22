using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Infrastructure;
using GoodHamburger.Infrastructure.Persistence;
using GoodHamburger.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;

namespace GoodHamburger.Infrastructure.Test;

public class RepositoryTest : IDisposable
{
    private readonly GoodHamburgerContext _context;
    private readonly OrderRepository _orderRepository;
    private readonly SqliteConnection _connection;

    public RepositoryTest()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<GoodHamburgerContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new GoodHamburgerContext(options);
        _context.Database.EnsureCreated();

        _orderRepository = new OrderRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistOrder()
    {
        // Arrange
        var order = new Order 
        { 
            CustomerName = "Repo Test",
            Items = new List<OrderItem> 
            { 
                new OrderItem { MenuItemId = Guid.NewGuid(), Name = "Test Item", Price = 10, Type = ItemType.Sandwich } 
            }
        };
        order.CalculateTotals();

        // Act
        await _orderRepository.AddAsync(order);

        // Assert
        var persisted = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == order.Id);
        persisted.Should().NotBeNull();
        persisted!.CustomerName.Should().Be("Repo Test");
        persisted.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStatus()
    {
        // Arrange
        var order = new Order { CustomerName = "Status Update" };
        await _orderRepository.AddAsync(order);
        await _context.SaveChangesAsync();

        // Act
        order.Status = OrderStatus.Finished;
        await _orderRepository.UpdateAsync(order);
        await _context.SaveChangesAsync();

        // Assert
        var updated = await _orderRepository.GetByIdAsync(order.Id);
        updated!.Status.Should().Be(OrderStatus.Finished);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
