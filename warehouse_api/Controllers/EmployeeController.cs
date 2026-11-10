using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using warehouse_api.Common.Responses;
using warehouse_api.DTOs.Employees;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    private readonly IValidator<CreateEmployeeRequestDto>
        _createValidator;

    private readonly IValidator<UpdateEmployeeRequestDto>
        _updateValidator;

    public EmployeesController(
        IEmployeeService employeeService,
        IValidator<CreateEmployeeRequestDto> createValidator,
        IValidator<UpdateEmployeeRequestDto> updateValidator)
    {
        _employeeService = employeeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();

        return Ok(new ApiResponse<List<EmployeeResponseDto>>
        {
            Data = employees,
            Message = "Employees retrieved successfully.",
            IsSuccess = true
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                ErrorMessages = new List<string>
                {
                    "Employee was not found."
                },
                IsSuccess = false
            });
        }

        return Ok(new ApiResponse<EmployeeResponseDto>
        {
            Data = employee,
            Message = "Employee retrieved successfully.",
            IsSuccess = true
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeRequestDto request)
    {
        var validationResult =
            await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var employee =
            await _employeeService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.EmpId },
            new ApiResponse<EmployeeResponseDto>
            {
                Data = employee,
                Message = "Employee created successfully.",
                IsSuccess = true
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateEmployeeRequestDto request)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var employee =
            await _employeeService.UpdateAsync(id, request);

        if (employee is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                ErrorMessages = new List<string>
                {
                    "Employee was not found."
                },
                IsSuccess = false
            });
        }

        return Ok(new ApiResponse<EmployeeResponseDto>
        {
            Data = employee,
            Message = "Employee updated successfully.",
            IsSuccess = true
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _employeeService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                ErrorMessages = new List<string>
                {
                    "Employee was not found."
                },
                IsSuccess = false
            });
        }

        return Ok(new ApiResponse<object?>
        {
            Data = null,
            Message = "Employee deactivated successfully.",
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