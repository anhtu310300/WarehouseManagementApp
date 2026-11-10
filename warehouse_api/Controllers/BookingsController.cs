using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using warehouse_api.Common.Responses;
using warehouse_api.DTOs.Bookings;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    private readonly IValidator<CreateBookingRequestDto>
        _createValidator;

    private readonly IValidator<UpdateBookingRequestDto>
        _updateValidator;

    private readonly IValidator<BookingCalendarRequestDto>
        _calendarValidator;

    public BookingsController(
        IBookingService bookingService,
        IValidator<CreateBookingRequestDto> createValidator,
        IValidator<UpdateBookingRequestDto> updateValidator,
        IValidator<BookingCalendarRequestDto> calendarValidator)
    {
        _bookingService = bookingService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _calendarValidator = calendarValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _bookingService.GetAllAsync();

        return Ok(
            new ApiResponse<List<BookingResponseDto>>
            {
                Data = bookings,
                Message = "Bookings retrieved successfully.",
                IsSuccess = true
            });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Booking was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<BookingResponseDto>
            {
                Data = booking,
                Message = "Booking retrieved successfully.",
                IsSuccess = true
            });
    }

    [HttpGet("calendar")]
    public async Task<IActionResult> GetCalendar(
        [FromQuery] BookingCalendarRequestDto request)
    {
        var validationResult =
            await _calendarValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var bookings =
            await _bookingService.GetCalendarAsync(request);

        return Ok(
            new ApiResponse<List<BookingCalendarResponseDto>>
            {
                Data = bookings,
                Message = "Booking calendar retrieved successfully.",
                IsSuccess = true
            });
    }

    [HttpPost]
    public async Task<IActionResult> Create( [FromBody] CreateBookingRequestDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        // We validate everything in the service layer, but if we don't have try catch here, the exception will be thrown and the API will return 500 Internal Server Error ???. We want to return 400 Bad Request with the error message

        try {
        var booking = await _bookingService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = booking.BookingId },
            new ApiResponse<BookingResponseDto>
            {
                Data = booking,
                Message = "Booking created successfully.",
                IsSuccess = true
            });
        } 
        catch (ArgumentException exception) 
        { 
            return BadRequest(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        exception.Message
                    },
                    IsSuccess = false
                });
        }

    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookingRequestDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        try 
        {
        var booking = await _bookingService.UpdateAsync(id, request);

        if (booking is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Booking was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<BookingResponseDto>
            {
                Data = booking,
                Message = "Booking updated successfully.",
                IsSuccess = true
            });
            
        } 
        catch (ArgumentException exception) 
        {
            return BadRequest(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        exception.Message
                    },
                    IsSuccess = false
                });
        }

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _bookingService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Booking was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<object?>
            {
                Data = null,
                Message = "Booking deleted successfully.",
                IsSuccess = true
            });
    }

    private static IActionResult ValidationFailure(ValidationResult validationResult)
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
