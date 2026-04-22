using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces.Repositories;

namespace GoodHamburger.Application.Services;

public interface IMenuService
{
    Task<IEnumerable<MenuItemResponse>> GetMenuAsync();
}

public class MenuService : IMenuService
{
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuService(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<IEnumerable<MenuItemResponse>> GetMenuAsync()
    {
        var items = await _menuItemRepository.GetAllAsync();
        return items.Select(i => new MenuItemResponse(i.Id, i.Name, i.Price, i.Type.ToString(), i.ImageUrl));
    }
}
