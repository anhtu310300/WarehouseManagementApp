using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using warehouse_api.Common.Responses;
using warehouse_api.DTOs.Payments;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    private readonly IValidator<CreatePaymentRequestDto>
        _createValidator;

    private readonly IValidator<UpdatePaymentRequestDto>
        _updateValidator;

    public PaymentsController(
        IPaymentService paymentService,
        IValidator<CreatePaymentRequestDto> createValidator,
        IValidator<UpdatePaymentRequestDto> updateValidator)
    {
        _paymentService = paymentService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var payments = await _paymentService.GetAllAsync();

        return Ok(
            new ApiResponse<List<PaymentResponseDto>>
            {
                Data = payments,
                Messages = new List<string>
                {
                    "Payments retrieved successfully."
                }
            });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);

        if (payment is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    Messages = new List<string>
                    {
                        "Payment was not found."
                    }
                });
        }

        return Ok(
            new ApiResponse<PaymentResponseDto>
            {
                Data = payment,
                Messages = new List<string>
                {
                    "Payment retrieved successfully."
                }
            });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequestDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        try
        {
            var payment = await _paymentService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = payment.PaymentId },
                new ApiResponse<PaymentResponseDto>
                {
                    Data = payment,
                    Messages = new List<string>
                    {
                        "Payment created successfully."
                    }
                });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(
                new ApiResponse<object?>
                {
                    Data = null,
                    Messages = new List<string>
                    {
                        exception.Message
                    }
                });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentRequestDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        try
        {
            var payment = await _paymentService.UpdateAsync(id, request);

            if (payment is null)
            {
                return NotFound(
                    new ApiResponse<object?>
                    {
                        Data = null,
                        Messages = new List<string>
                        {
                            "Payment was not found."
                        }
                    });
            }

            return Ok(
                new ApiResponse<PaymentResponseDto>
                {
                    Data = payment,
                    Messages = new List<string>
                    {
                        "Payment updated successfully."
                    }
                });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(
                new ApiResponse<object?>
                {
                    Data = null,
                    Messages = new List<string>
                    {
                        exception.Message
                    }
                });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _paymentService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    Messages = new List<string>
                    {
                        "Payment was not found."
                    }
                });
        }

        return Ok(
            new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Payment deleted successfully."
                }
            });
    }

    private static IActionResult ValidationFailure(ValidationResult validationResult)
    {
        var messages = validationResult.Errors.Select(error => error.ErrorMessage).Distinct().ToList();

        return new BadRequestObjectResult(
            new ApiResponse<object?>
            {
                Data = null,
                Messages = messages
            });
    }
}