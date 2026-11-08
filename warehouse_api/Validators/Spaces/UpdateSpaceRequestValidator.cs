using FluentValidation;
using warehouse_api.DTOs.Spaces;

namespace warehouse_api.Validators.Spaces
{
    public class UpdateSpaceRequestValidator
        : AbstractValidator<UpdateSpaceRequestDto>
    {
        public UpdateSpaceRequestValidator()
        {
            RuleFor(request => request.WarehouseId)
                .GreaterThan(0)
                .WithMessage(
                    "Warehouse ID must be greater than zero.");

            RuleFor(request => request.SpaceName)
                .NotEmpty()
                .WithMessage("Space name is required.")
                .MaximumLength(80)
                .WithMessage(
                    "Space name must not exceed 80 characters.");

            RuleFor(request => request.AreaSqft)
                .GreaterThan(0)
                .WithMessage(
                    "Space area must be greater than zero.");

            RuleFor(request => request.Description)
                .MaximumLength(250)
                .WithMessage(
                    "Description must not exceed 250 characters.");
        }
    }
}