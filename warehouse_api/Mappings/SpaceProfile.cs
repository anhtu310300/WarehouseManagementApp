using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Spaces;

namespace warehouse_api.Mappings
{
    public class SpaceProfile : Profile
    {
        public SpaceProfile()
        {
            CreateMap<Space, SpaceResponseDto>();

            CreateMap<CreateSpaceRequestDto, Space>();

            CreateMap<UpdateSpaceRequestDto, Space>();
        }
    }
}