using FluentValidation;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do cliente não pode exceder 100 caracteres.");

        RuleFor(x => x.MenuItemIds)
            .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.")
            .Must(x => x.Count <= 10).WithMessage("O pedido não pode ter mais do que 10 itens.");
    }
}

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("O nome do cliente é obrigatório.");

        RuleFor(x => x.MenuItemIds)
            .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.");
    }
}
