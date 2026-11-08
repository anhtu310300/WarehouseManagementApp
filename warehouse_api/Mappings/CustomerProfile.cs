using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Customers;

namespace warehouse_api.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerResponseDto>();

            CreateMap<CreateCustomerRequestDto, Customer>();

            CreateMap<UpdateCustomerRequestDto, Customer>();
        }
    }
}
