using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemResponse>>> GetMenu()
    {
        return Ok(await _menuService.GetMenuAsync());
    }
}
