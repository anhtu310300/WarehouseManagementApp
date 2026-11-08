using FluentValidation;
using warehouse_api.DTOs.Payments;

namespace warehouse_api.Validators.Payments
{
    public class UpdatePaymentRequestValidator
        : AbstractValidator<UpdatePaymentRequestDto>
    {
        public UpdatePaymentRequestValidator()
        {
            RuleFor(request => request.BookingId)
                .GreaterThan(0)
                .WithMessage(
                    "Booking ID must be greater than zero.");

            RuleFor(request => request.PaymentAmount)
                .GreaterThan(0)
                .WithMessage(
                    "Payment amount must be greater than zero.");

            RuleFor(request => request.PaymentMode)
                .NotEmpty()
                .WithMessage("Payment mode is required.")
                .MaximumLength(50)
                .WithMessage(
                    "Payment mode must not exceed 50 characters.");

            RuleFor(request => request.PaymentNarration)
                .NotEmpty()
                .WithMessage("Payment narration is required.")
                .MaximumLength(500)
                .WithMessage(
                    "Payment narration must not exceed 500 characters.");

            RuleFor(request => request.Status)
                .NotEmpty()
                .WithMessage("Payment status is required.")
                .MaximumLength(50)
                .WithMessage(
                    "Payment status must not exceed 50 characters.");

            RuleFor(request => request.ReceivedId)
                .GreaterThan(0)
                .WithMessage(
                    "Receiving employee ID must be greater than zero.");
        }
    }
}