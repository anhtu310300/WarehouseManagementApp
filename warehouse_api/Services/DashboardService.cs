using warehouse_api.DTOs.Dashboards;
using warehouse_api.Repositories.Interfaces;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IWarehouseRepository
            _warehouseRepository;

        private readonly ISpaceRepository
            _spaceRepository;

        private readonly ICustomerRepository
            _customerRepository;

        private readonly IEmployeeRepository
            _employeeRepository;

        private readonly IBookingRepository
            _bookingRepository;

        private readonly IPaymentRepository
            _paymentRepository;

        public DashboardService(
            IWarehouseRepository warehouseRepository,
            ISpaceRepository spaceRepository,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IBookingRepository bookingRepository,
            IPaymentRepository paymentRepository)
        {
            _warehouseRepository = warehouseRepository;
            _spaceRepository = spaceRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<WarehouseDashboardResponseDto?>
            GetWarehouseDashboardAsync(
                int warehouseId,
                DateTime dashboardDate)
        {
            var warehouse =
                await _warehouseRepository.GetByIdAsync(
                    warehouseId);

            if (warehouse is null)
            {
                return null;
            }

            var selectedDate = dashboardDate.Date;

            var spaces =
                await _spaceRepository
                    .GetActiveByWarehouseIdAsync(
                        warehouseId);

            var bookings =
                await _bookingRepository
                    .GetOverlappingBookingsAsync(
                        warehouseId,
                        selectedDate,
                        selectedDate);

            var bookedAreaBySpace = bookings
                .Where(booking => booking.SpaceId.HasValue)
                .GroupBy(booking =>
                    booking.SpaceId.GetValueOrDefault())
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(booking =>
                        booking.SqftAreaNeeded));

            var bookingCountBySpace = bookings
                .Where(booking => booking.SpaceId.HasValue)
                .GroupBy(booking =>
                    booking.SpaceId.GetValueOrDefault())
                .ToDictionary(
                    group => group.Key,
                    group => group.Count());

            var spaceDashboardItems =
                new List<WarehouseSpaceDashboardDto>();

            foreach (var space in spaces)
            {
                bookedAreaBySpace.TryGetValue(
                    space.SpaceId,
                    out var bookedArea);

                bookingCountBySpace.TryGetValue(
                    space.SpaceId,
                    out var activeBookings);

                var availableArea = Math.Max(
                    0,
                    space.AreaSqft - bookedArea);

                spaceDashboardItems.Add(
                    new WarehouseSpaceDashboardDto
                    {
                        SpaceId = space.SpaceId,
                        SpaceName = space.SpaceName,
                        TotalAreaSqft = space.AreaSqft,
                        BookedAreaSqft = bookedArea,
                        AvailableAreaSqft = availableArea,
                        ActiveBookings = activeBookings,
                        IsAvailable = availableArea > 0
                    });
            }

            return new WarehouseDashboardResponseDto
            {
                WarehouseId = warehouse.WarehouseId,
                WarehouseName = warehouse.Title,
                DashboardDate = selectedDate,
                WarehouseAreaSqft = warehouse.TotalArea,
                TotalSpaces = spaceDashboardItems.Count,
                TotalSpaceAreaSqft =
                    spaceDashboardItems.Sum(space =>
                        space.TotalAreaSqft),
                BookedAreaSqft =
                    spaceDashboardItems.Sum(space =>
                        space.BookedAreaSqft),
                AvailableAreaSqft =
                    spaceDashboardItems.Sum(space =>
                        space.AvailableAreaSqft),
                ActiveBookings = bookings.Count,
                Spaces = spaceDashboardItems
            };
        }

        public async Task<OwnerDashboardResponseDto>
            GetOwnerDashboardAsync()
        {
            var warehouses =
                await _warehouseRepository.GetAllAsync();

            var spaces =
                await _spaceRepository.GetAllAsync();

            var customers =
                await _customerRepository.GetAllAsync();

            var employees =
                await _employeeRepository.GetAllAsync();

            var bookings =
                await _bookingRepository.GetAllAsync();

            var payments =
                await _paymentRepository.GetAllAsync();

            var completedPayments = payments
                .Where(payment =>
                    string.Equals(
                        payment.Status,
                        "Completed",
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

            var totalBookingAmount =
                bookings.Sum(booking =>
                    booking.TotalAmount);

            var totalPaidAmount =
                completedPayments.Sum(payment =>
                    payment.PaymentAmount);

            var remainingAmount = Math.Max(
                0,
                totalBookingAmount - totalPaidAmount);

            return new OwnerDashboardResponseDto
            {
                TotalWarehouses = warehouses.Count,
                TotalSpaces = spaces.Count,
                ActiveSpaces = spaces.Count(space =>
                    space.IsActive),
                InactiveSpaces = spaces.Count(space =>
                    !space.IsActive),
                TotalCustomers = customers.Count,
                TotalEmployees = employees.Count,
                ActiveEmployees = employees.Count(employee =>
                    employee.IsActive),
                TotalBookings = bookings.Count,
                TotalPayments = payments.Count,
                CompletedPayments =
                    completedPayments.Count,
                TotalBookingAmount = totalBookingAmount,
                TotalPaidAmount = totalPaidAmount,
                RemainingAmount = remainingAmount
            };
        }
    }
}
