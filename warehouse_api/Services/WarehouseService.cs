using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Warehouses;
using warehouse_api.Repositories.Interfaces;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository
            _warehouseRepository;

        private readonly IMapper _mapper;

        public WarehouseService(
            IWarehouseRepository warehouseRepository,
            IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
        }

        public async Task<List<WarehouseResponseDto>>
            GetAllAsync()
        {
            var warehouses =
                await _warehouseRepository.GetAllAsync();

            return _mapper.Map<List<WarehouseResponseDto>>(
                warehouses);
        }

        public async Task<WarehouseResponseDto?> GetByIdAsync(
            int warehouseId)
        {
            var warehouse =
                await _warehouseRepository.GetByIdAsync(
                    warehouseId);

            if (warehouse is null)
            {
                return null;
            }

            return _mapper.Map<WarehouseResponseDto>(
                warehouse);
        }

        public async Task<WarehouseResponseDto> CreateAsync(
            CreateWarehouseRequestDto request)
        {
            var warehouse =
                _mapper.Map<Warehouse>(request);

            var createdWarehouse =
                await _warehouseRepository.CreateAsync(
                    warehouse);

            return _mapper.Map<WarehouseResponseDto>(
                createdWarehouse);
        }

        public async Task<WarehouseResponseDto?> UpdateAsync(
            int warehouseId,
            UpdateWarehouseRequestDto request)
        {
            var warehouse =
                await _warehouseRepository.GetByIdAsync(
                    warehouseId);

            if (warehouse is null)
            {
                return null;
            }

            _mapper.Map(request, warehouse);

            var updatedWarehouse =
                await _warehouseRepository.UpdateAsync(
                    warehouse);

            return _mapper.Map<WarehouseResponseDto>(
                updatedWarehouse);
        }

        public async Task<bool> DeleteAsync(int warehouseId)
        {
            return await _warehouseRepository.DeleteAsync(
                warehouseId);
        }
    }
}
