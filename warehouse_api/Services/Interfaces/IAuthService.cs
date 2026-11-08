using warehouse_api.DTOs.Auth;

namespace warehouse_api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
