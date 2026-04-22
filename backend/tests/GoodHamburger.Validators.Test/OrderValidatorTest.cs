using FluentValidation.TestHelper;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Validators;
using Xunit;

namespace GoodHamburger.Validators.Test;

public class OrderValidatorTest
{
    private readonly CreateOrderRequestValidator _validator;

    public OrderValidatorTest()
    {
        _validator = new CreateOrderRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_CustomerName_Is_Empty()
    {
        var model = new CreateOrderRequest("", new List<Guid> { Guid.NewGuid() });
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CustomerName);
    }

    [Fact]
    public void Should_Have_Error_When_MenuItemIds_Is_Empty()
    {
        var model = new CreateOrderRequest("John", new List<Guid>());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.MenuItemIds);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Request_Is_Valid()
    {
        var model = new CreateOrderRequest("John Doe", new List<Guid> { Guid.NewGuid() });
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
