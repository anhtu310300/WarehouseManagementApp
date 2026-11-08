using FluentValidation;
using warehouse_api.DTOs.Employees;


namespace warehouse_api.Validators.Employees
{
    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequestDto> // built -in validator class from FluentValidation library
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(request => request.EmpName).NotEmpty().WithMessage("Employee name is required.")
                .MaximumLength(50).WithMessage("Employee name must not exceed 50 characters.");

            RuleFor(request => request.WarehouseAssignedId).GreaterThan(0).WithMessage("Warehouse ID must be greater than zero.");

            RuleFor(request => request.MobileNo).NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"^\d{10}$").WithMessage("Mobile number must contain exactly 10 digits.");

            RuleFor(request => request.Role).NotEmpty().WithMessage("Role is required.")
                .MaximumLength(50).WithMessage("Role must not exceed 50 characters.");

            RuleFor(request => request.UserName).NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

            RuleFor(request => request.Password).NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must contain at least 8 characters.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
        }
    }
}

// instead of validating directly in the domain model, use separate DTOs for requests and responses, then apply validation rules to the request DTOs. 
