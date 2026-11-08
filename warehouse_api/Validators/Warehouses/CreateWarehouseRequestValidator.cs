using FluentValidation;
using warehouse_api.DTOs.Warehouses;

namespace warehouse_api.Validators.Warehouses
{
    public class CreateWarehouseRequestValidator
        : AbstractValidator<CreateWarehouseRequestDto>
    {
        public CreateWarehouseRequestValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .WithMessage("Warehouse title is required.")
                .MaximumLength(50)
                .WithMessage(
                    "Warehouse title must not exceed 50 characters.");

            RuleFor(request => request.Address1)
                .NotEmpty()
                .WithMessage("Address line 1 is required.")
                .MaximumLength(500)
                .WithMessage(
                    "Address line 1 must not exceed 500 characters.");

            RuleFor(request => request.Address2)
                .NotEmpty()
                .WithMessage("Address line 2 is required.")
                .MaximumLength(1000)
                .WithMessage(
                    "Address line 2 must not exceed 1000 characters.");

            RuleFor(request => request.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(50)
                .WithMessage(
                    "City must not exceed 50 characters.");

            RuleFor(request => request.TotalArea)
                .GreaterThan(0)
                .WithMessage(
                    "Total area must be greater than zero.");
        }
    }
}