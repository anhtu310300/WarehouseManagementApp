using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using warehouse_api.Common.Responses;
using warehouse_api.DTOs.Customers;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    private readonly IValidator<CreateCustomerRequestDto>
        _createValidator;

    private readonly IValidator<UpdateCustomerRequestDto>
        _updateValidator;

    public CustomersController(
        ICustomerService customerService,
        IValidator<CreateCustomerRequestDto> createValidator,
        IValidator<UpdateCustomerRequestDto> updateValidator)
    {
        _customerService = customerService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers =
            await _customerService.GetAllAsync();

        return Ok(
            new ApiResponse<List<CustomerResponseDto>>
            {
                Data = customers,
                Message = "Customers retrieved successfully.",
                IsSuccess = true
            });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer =
            await _customerService.GetByIdAsync(id);

        if (customer is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Customer was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<CustomerResponseDto>
            {
                Data = customer,
                Message = "Customer retrieved successfully.",
                IsSuccess = true
            });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCustomerRequestDto request)
    {
        var validationResult =
            await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var customer =
            await _customerService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.CustomerId },
            new ApiResponse<CustomerResponseDto>
            {
                Data = customer,
                Message = "Customer created successfully.",
                IsSuccess = true
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCustomerRequestDto request)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var customer =
            await _customerService.UpdateAsync(id, request);

        if (customer is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Customer was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<CustomerResponseDto>
            {
                Data = customer,
                Message = "Customer updated successfully.",
                IsSuccess = true
            });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _customerService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Customer was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<object?>
            {
                Data = null,
                Message = "Customer deleted successfully.",
                IsSuccess = true
            });
    }

    private static IActionResult ValidationFailure(
        ValidationResult validationResult)
    {
        var messages = validationResult.Errors
            .Select(error => error.ErrorMessage)
            .Distinct()
            .ToList();

        return new BadRequestObjectResult(
            new ApiResponse<object?>
            {
                Data = null,
                ErrorMessages = messages,
                IsSuccess = false
            });
    }
}
