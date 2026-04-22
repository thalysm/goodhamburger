using GoodHamburger.Application.Interfaces.Repositories;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly GoodHamburgerContext _context;

    public OrderRepository(GoodHamburgerContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders.Include(o => o.Items).ToListAsync();
    }

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        // O objeto já está sendo rastreado pelo EF, basta salvar
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}

public class MenuItemRepository : IMenuItemRepository
{
    private readonly GoodHamburgerContext _context;

    public MenuItemRepository(GoodHamburgerContext context)
    {
        _context = context;
    }

    public async Task<MenuItem?> GetByIdAsync(Guid id)
    {
        return await _context.MenuItems.FindAsync(id);
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync()
    {
        return await _context.MenuItems.ToListAsync();
    }
}
