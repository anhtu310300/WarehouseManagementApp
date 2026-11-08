using FluentValidation;
using warehouse_api.DTOs.Bookings;

namespace warehouse_api.Validators.Bookings
{
    public class CreateBookingRequestValidator
        : AbstractValidator<CreateBookingRequestDto>
    {
        public CreateBookingRequestValidator()
        {
            RuleFor(request => request.CustomerId)
                .GreaterThan(0)
                .WithMessage(
                    "Customer ID must be greater than zero.");

            RuleFor(request => request.WarehouseId)
                .GreaterThan(0)
                .WithMessage(
                    "Warehouse ID must be greater than zero.");

            RuleFor(request => request.SpaceId)
                .GreaterThan(0)
                .WithMessage(
                    "Space ID must be greater than zero.");

            RuleFor(request => request.BookingFromDate)
                .NotEmpty()
                .WithMessage(
                    "Booking start date is required.");

            RuleFor(request => request.BookingTillDate)
                .NotEmpty()
                .WithMessage(
                    "Booking end date is required.")
                .GreaterThanOrEqualTo(
                    request => request.BookingFromDate)
                .WithMessage(
                    "Booking end date must be greater than or equal to the start date.");

            RuleFor(request => request.SqftAreaNeeded)
                .GreaterThan(0)
                .WithMessage(
                    "Required area must be greater than zero.");

            RuleFor(request => request.TotalAmount)
                .GreaterThan(0)
                .WithMessage(
                    "Total amount must be greater than zero.");

            RuleFor(request => request.BookingDoneBy)
                .GreaterThan(0)
                .WithMessage(
                    "Booking employee ID must be greater than zero.");
        }
    }
}