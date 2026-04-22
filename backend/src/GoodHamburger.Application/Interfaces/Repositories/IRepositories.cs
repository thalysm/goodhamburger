using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Guid id);
}

public interface IMenuItemRepository
{
    Task<MenuItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<MenuItem>> GetAllAsync();
}
