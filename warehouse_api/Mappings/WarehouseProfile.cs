using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Warehouses;

namespace warehouse_api.Mappings
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseResponseDto>();

            CreateMap<CreateWarehouseRequestDto, Warehouse>();

            CreateMap<UpdateWarehouseRequestDto, Warehouse>();
        }
    }
}
