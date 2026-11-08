using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Bookings;

namespace warehouse_api.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingResponseDto>();

            CreateMap<CreateBookingRequestDto, Booking>();

            CreateMap<UpdateBookingRequestDto, Booking>();
        }
    }
}