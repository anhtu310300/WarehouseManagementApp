using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Employees;

namespace warehouse_api.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        { 
            CreateMap<Employee, EmployeeResponseDto>();

            CreateMap<CreateEmployeeRequestDto, Employee>();

            CreateMap<UpdateEmployeeRequestDto, Employee>();
        }
    }
}
