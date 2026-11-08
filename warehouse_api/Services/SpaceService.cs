using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Spaces;
using warehouse_api.Repositories;
using warehouse_api.Repositories.Interfaces;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly ISpaceRepository _spaceRepository;

        private readonly IBookingRepository _bookingRepository;

        private readonly IMapper _mapper;

        public SpaceService(
            ISpaceRepository spaceRepository,
            IBookingRepository bookingRepository,
            IMapper mapper)
        {
            _spaceRepository = spaceRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<List<SpaceResponseDto>> GetAllAsync()
        {
            var spaces = await _spaceRepository.GetAllAsync();
            return _mapper.Map<List<SpaceResponseDto>>(spaces);
        }

        public async Task<SpaceResponseDto?> GetByIdAsync(
            int spaceId)
        {
            var space =
                await _spaceRepository.GetByIdAsync(spaceId);

            if (space is null)
            {
                return null;
            }

            return _mapper.Map<SpaceResponseDto>(space);
        }

        // If no Booking -> BookedArea = 0, AvailableArea = AreaSqft, IsAvailable = true
        // AvailableArea must not be negative, if BookedArea > AreaSqft, AvailableArea = 0, IsAvailable = false
        // availableArea > 0 => IsAvailable = true, availableArea <= 0 => IsAvailable = false

        public async Task<List<SpaceAvailabilityResponseDto>> GetAvailabilityAsync(int warehouseId, DateTime fromDate, DateTime tillDate)
        {
            var spaces = await _spaceRepository.GetActiveByWarehouseIdAsync(warehouseId);
            var bookings = await _bookingRepository.GetOverlappingBookingsAsync(warehouseId, fromDate, tillDate);
            var bookedAreaBySpace = bookings.Where(booking => booking.SpaceId.HasValue).GroupBy(
                booking => booking.SpaceId.GetValueOrDefault()).ToDictionary(
                    group => group.Key,
                    group => group.Sum(booking => booking.SqftAreaNeeded));
            // Remove SpaceId that is null, then gather booking based on SpaceId, then sum the SqftAreaNeeded for each group,
            // and store in a dictionary with SpaceId as key and total booked area as value.

            var result = new List<SpaceAvailabilityResponseDto>();

            foreach ( var space in spaces)
            {
                // Calculate the available area for each active space.
                // If the space has no overlapping bookings, bookedArea defaults to 0.
                // Available area cannot be negative.
                // A space is available when it has remaining area.
                bookedAreaBySpace.TryGetValue(space.SpaceId, out var bookedArea);
                var availableArea = Math.Max(0, space.AreaSqft - bookedArea);

                result.Add(new SpaceAvailabilityResponseDto
                {
                    SpaceId = space.SpaceId,
                    WarehouseId = space.WarehouseId,
                    SpaceName = space.SpaceName,
                    TotalAreaSqft = space.AreaSqft,
                    BookedAreaSqft = bookedArea,
                    AvailableAreaSqft = availableArea,
                    IsAvailable = availableArea > 0
                });
            }
            return result;
        }

        

        public async Task<SpaceResponseDto> CreateAsync(
            CreateSpaceRequestDto request)
        {
            var space = _mapper.Map<Space>(request);

            space.IsActive = true;

            var createdSpace =
                await _spaceRepository.CreateAsync(space);

            return _mapper.Map<SpaceResponseDto>(
                createdSpace);
        }

        public async Task<SpaceResponseDto?> UpdateAsync(
            int spaceId,
            UpdateSpaceRequestDto request)
        {
            var space =
                await _spaceRepository.GetByIdAsync(spaceId);

            if (space is null)
            {
                return null;
            }

            _mapper.Map(request, space);

            var updatedSpace =
                await _spaceRepository.UpdateAsync(space);

            return _mapper.Map<SpaceResponseDto>(
                updatedSpace);
        }

        public async Task<bool> DeleteAsync(int spaceId)
        {
            return await _spaceRepository.DeleteAsync(spaceId);
        }
    }
}
