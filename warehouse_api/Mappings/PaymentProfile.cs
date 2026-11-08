using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Payments;

namespace warehouse_api.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentResponseDto>();

            CreateMap<CreatePaymentRequestDto, Payment>();

            CreateMap<UpdatePaymentRequestDto, Payment>();
        }
    }
}