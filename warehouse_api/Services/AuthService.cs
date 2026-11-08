using AutoMapper;
using Microsoft.AspNetCore.Identity;
using warehouse_api.Domain;
using warehouse_api.DTOs.Auth;
using warehouse_api.Repositories.Interfaces;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<Employee> _passwordHasher;
    private readonly IMapper _mapper;

    public AuthService(
        IAuthRepository authRepository,
        IPasswordHasher<Employee> passwordHasher,
        IMapper mapper)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto request)
    {
        var userName = request.UserName.Trim();

        var employee =
            await _authRepository.GetByUserNameAsync(userName);

        if (employee is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid username or password.");
        }

        var passwordResult =
            _passwordHasher.VerifyHashedPassword(
                employee,
                employee.PasswordHash,
                request.Password);

        if (passwordResult ==
            PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException(
                "Invalid username or password.");
        }

        if (!employee.IsActive)
        {
            throw new UnauthorizedAccessException(
                "The account is inactive.");
        }

        return _mapper.Map<LoginResponseDto>(employee);
    }
}