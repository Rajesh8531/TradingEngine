using System;
using System.Collections.Generic;
using System.Text;
using Application.Command;
using FluentValidation;

namespace Application.Validators
{
    internal class CreateBalanceValidator : AbstractValidator<CreateBalanceCommand>
    {
        public CreateBalanceValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(userId => userId != Guid.Empty).WithMessage("UserId cannot be an empty GUID.");

            RuleFor(x => x.AssetSymbol)
                .NotEmpty().WithMessage("AssetSymbol is required.")
                .MaximumLength(10).WithMessage("AssetSymbol should not exceed 10 characters.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be a positive number.");
        }
    }
}
