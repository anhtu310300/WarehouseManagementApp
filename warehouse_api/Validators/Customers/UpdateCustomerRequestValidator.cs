using FluentValidation;
using warehouse_api.DTOs.Customers;

namespace warehouse_api.Validators.Customers
{
    public class UpdateCustomerRequestValidator
        : AbstractValidator<UpdateCustomerRequestDto>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(request => request.CustName)
                .NotEmpty()
                .WithMessage("Customer name is required.")
                .MaximumLength(50)
                .WithMessage(
                    "Customer name must not exceed 50 characters.");

            RuleFor(request => request.Address)
                .MaximumLength(50)
                .WithMessage(
                    "Address must not exceed 50 characters.");

            RuleFor(request => request.Mobile)
                .NotEmpty()
                .WithMessage("Mobile number is required.")
                .MaximumLength(50)
                .WithMessage(
                    "Mobile number must not exceed 50 characters.");

            RuleFor(request => request.Email)
                .MaximumLength(50)
                .WithMessage(
                    "Email must not exceed 50 characters.")
                .EmailAddress()
                .WithMessage("Email format is invalid.")
                .When(request =>
                    !string.IsNullOrWhiteSpace(request.Email));

            RuleFor(request => request.GstNo)
                .MaximumLength(50)
                .WithMessage(
                    "GST number must not exceed 50 characters.");
        }
    }
}