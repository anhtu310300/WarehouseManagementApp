using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using warehouse_api.Common.Responses;
using warehouse_api.DTOs.Spaces;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Controllers;

[ApiController]
[Route("api/spaces")]
public class SpacesController : ControllerBase
{
    private readonly ISpaceService _spaceService;

    private readonly IValidator<CreateSpaceRequestDto>
        _createValidator;

    private readonly IValidator<UpdateSpaceRequestDto>
        _updateValidator;
    private readonly IValidator<SpaceAvailabilityRequestDto>
    _availabilityValidator;

    public SpacesController(
        ISpaceService spaceService,
        IValidator<CreateSpaceRequestDto> createValidator,
        IValidator<UpdateSpaceRequestDto> updateValidator,
        IValidator<SpaceAvailabilityRequestDto>
        availabilityValidator)
    {
        _spaceService = spaceService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _availabilityValidator = availabilityValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var spaces = await _spaceService.GetAllAsync();

        return Ok(
            new ApiResponse<List<SpaceResponseDto>>
            {
                Data = spaces,
                Messages = new List<string>
                {
                    "Spaces retrieved successfully."
                }
            });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var space = await _spaceService.GetByIdAsync(id);

        if (space is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    Messages = new List<string>
                    {
                        "Space was not found."
                    }
                });
        }

        return Ok(
            new ApiResponse<SpaceResponseDto>
            {
                Data = space,
                Messages = new List<string>
                {
                    "Space retrieved successfully."
                }
            });
    }

    [HttpGet("availability")]
    public async Task<IActionResult> GetAvailability(
    [FromQuery] SpaceAvailabilityRequestDto request)
    {
        var validationResult =
            await _availabilityValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var spaces =
            await _spaceService.GetAvailabilityAsync(
                request.WarehouseId,
                request.FromDate,
                request.TillDate);

        return Ok(
            new ApiResponse<List<SpaceAvailabilityResponseDto>>
            {
                Data = spaces,
                Messages = new List<string>
                {
                "Space availability retrieved successfully."
                }
            });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSpaceRequestDto request)
    {
        var validationResult =
            await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var space =
            await _spaceService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = space.SpaceId },
            new ApiResponse<SpaceResponseDto>
            {
                Data = space,
                Messages = new List<string>
                {
                    "Space created successfully."
                }
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateSpaceRequestDto request)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var space =
            await _spaceService.UpdateAsync(id, request);

        if (space is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    Messages = new List<string>
                    {
                        "Space was not found."
                    }
                });
        }

        return Ok(
            new ApiResponse<SpaceResponseDto>
            {
                Data = space,
                Messages = new List<string>
                {
                    "Space updated successfully."
                }
            });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _spaceService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    Messages = new List<string>
                    {
                        "Space was not found."
                    }
                });
        }

        return Ok(
            new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Space deactivated successfully."
                }
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
                Messages = messages
            });
    }
}