using FluentValidation;
using warehouse_api.DTOs.Spaces;

namespace warehouse_api.Validators.Spaces
{
    public class SpaceAvailabilityRequestValidator
        : AbstractValidator<SpaceAvailabilityRequestDto>
    {
        public SpaceAvailabilityRequestValidator()
        {
            RuleFor(request => request.WarehouseId)
                .GreaterThan(0)
                .WithMessage(
                    "Warehouse ID must be greater than zero.");

            RuleFor(request => request.FromDate)
                .NotEmpty()
                .WithMessage("From date is required.");

            RuleFor(request => request.TillDate)
                .NotEmpty()
                .WithMessage("Till date is required.")
                .GreaterThanOrEqualTo(
                    request => request.FromDate)
                .WithMessage(
                    "Till date must be greater than or equal to from date.");
        }
    }
}
