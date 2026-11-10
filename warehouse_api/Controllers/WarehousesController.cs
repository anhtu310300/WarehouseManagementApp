using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using warehouse_api.Common.Responses;
using warehouse_api.DTOs.Warehouses;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    private readonly IValidator<CreateWarehouseRequestDto>
        _createValidator;

    private readonly IValidator<UpdateWarehouseRequestDto>
        _updateValidator;

    public WarehousesController(
        IWarehouseService warehouseService,
        IValidator<CreateWarehouseRequestDto> createValidator,
        IValidator<UpdateWarehouseRequestDto> updateValidator)
    {
        _warehouseService = warehouseService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var warehouses =
            await _warehouseService.GetAllAsync();

        return Ok(
            new ApiResponse<List<WarehouseResponseDto>>
            {
                Data = warehouses,
                Message = "Warehouses retrieved successfully.",
                IsSuccess = true
            });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var warehouse =
            await _warehouseService.GetByIdAsync(id);

        if (warehouse is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Warehouse was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<WarehouseResponseDto>
            {
                Data = warehouse,
                Message = "Warehouse retrieved successfully.",
                IsSuccess = true
            });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateWarehouseRequestDto request)
    {
        var validationResult =
            await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var warehouse =
            await _warehouseService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = warehouse.WarehouseId },
            new ApiResponse<WarehouseResponseDto>
            {
                Data = warehouse,
                Message = "Warehouse created successfully.",
                IsSuccess = true
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateWarehouseRequestDto request)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var warehouse =
            await _warehouseService.UpdateAsync(id, request);

        if (warehouse is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Warehouse was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<WarehouseResponseDto>
            {
                Data = warehouse,
                Message = "Warehouse updated successfully.",
                IsSuccess = true
            });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _warehouseService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Warehouse was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<object?>
            {
                Data = null,
                Message = "Warehouse deleted successfully.",
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
