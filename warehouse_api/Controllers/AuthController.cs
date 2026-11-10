using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using warehouse_api.Common.Responses;
using warehouse_api.DTOs.Auth;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<LoginRequestDto>
            _loginValidator;

        public AuthController(
            IAuthService authService,
            IValidator<LoginRequestDto> loginValidator)
        {
            _authService = authService;
            _loginValidator = loginValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login ([FromBody] LoginRequestDto request)
        {
            var validationResult =
            await _loginValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var messages = validationResult.Errors
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToList();

                return BadRequest(new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = messages,
                    IsSuccess = false
                });
            }


            try
            {
                var loginReponse = await _authService.LoginAsync(request);

                return Ok(new ApiResponse<LoginResponseDto>
                {
                    Data = loginReponse,
                    Message = "Login successful",
                    IsSuccess = true
                });
            }

            catch (UnauthorizedAccessException exception)
            {
                return Unauthorized(new ApiResponse<object?>
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
    }
}
