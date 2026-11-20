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

        public async Task<List<SpaceResponseDto>> GetAllAsync(int? warehouseId = null)
        {
            var spaces = await _spaceRepository.GetAllAsync(warehouseId);
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

        public async Task<List<SpaceAvailabilityResponseDto>>
    GetAvailabilityAsync(
        int warehouseId,
        DateTime fromDate,
        DateTime tillDate)
        {
            var from = fromDate.Date;
            var till = tillDate.Date;

            // Controller now validates that from <= till, so we don't need to check it here.

            // 1. get all active spaces in the warehouse
            var spaces =
                await _spaceRepository.GetActiveByWarehouseIdAsync(
                    warehouseId);

            // 2. get all bookings that overlap with the requested date range.
            var bookings =
                await _bookingRepository.GetOverlappingBookingsAsync(
                    warehouseId,
                    from,
                    till);

            // Take the bookings that have a space assigned, and group them by spaceId.
            var bookingsBySpace = bookings
                .Where(booking => booking.SpaceId.HasValue)
                .ToLookup(booking => booking.SpaceId.GetValueOrDefault());

            var fromDay = DateOnly.FromDateTime(from).DayNumber;
            var tillDay = DateOnly.FromDateTime(till).DayNumber;

            var result = new List<SpaceAvailabilityResponseDto>();

            foreach (var space in spaces)
            {
                // 3. note the milestones of the sqft area taken for the space, and how much sqft area is taken on each date in the requested date range.
                var areaChanges = new SortedDictionary<int, long>();

                foreach (var booking in bookingsBySpace[space.SpaceId])
                {
                    var bookingFromDay =
                        DateOnly.FromDateTime(
                            booking.BookingFromDate).DayNumber;

                    var bookingTillDay =
                        DateOnly.FromDateTime(
                            booking.BookingTillDate).DayNumber;

                    var startDay = Math.Max(fromDay, bookingFromDay);
                    var endDay = Math.Min(tillDay, bookingTillDay);

                    if (startDay > endDay)
                    {
                        continue;
                    }

                    // Start date: increase the booked area by the sqftAreaNeeded of the booking.
                    areaChanges.TryGetValue(
                        startDay,
                        out var startChange);

                    areaChanges[startDay] =
                        startChange + booking.SqftAreaNeeded;

                    // End date: decrease the booked area by the sqftAreaNeeded of the booking.
                    var releaseDay = endDay + 1;

                    areaChanges.TryGetValue(
                        releaseDay,
                        out var releaseChange);

                    areaChanges[releaseDay] =
                        releaseChange - booking.SqftAreaNeeded;
                }

                // 4. Find the peak booked area for the space.
                long currentBookedArea = 0;
                long peakBookedArea = 0;

                foreach (var change in areaChanges)
                {
                    currentBookedArea += change.Value;

                    peakBookedArea = Math.Max(
                        peakBookedArea,
                        currentBookedArea);
                }

                // 5. Find the available area for the space throughout the requested booking period 
                var availableArea = Math.Max(
                    0L,
                    space.AreaSqft - peakBookedArea);

                result.Add(new SpaceAvailabilityResponseDto
                {
                    SpaceId = space.SpaceId,
                    WarehouseId = space.WarehouseId,
                    SpaceName = space.SpaceName,
                    TotalAreaSqft = space.AreaSqft,

                    BookedAreaSqft = checked((int)peakBookedArea),
                    AvailableAreaSqft = checked((int)availableArea),

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
