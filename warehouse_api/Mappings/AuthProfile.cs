using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Auth;

namespace warehouse_api.Mappings;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<Employee, LoginResponseDto>();
    }
}