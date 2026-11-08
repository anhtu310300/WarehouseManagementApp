using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Payments;
using warehouse_api.Repositories.Interfaces;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository
            _paymentRepository;

        private readonly IBookingRepository
            _bookingRepository;

        private readonly IEmployeeRepository
            _employeeRepository;

        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IBookingRepository bookingRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<PaymentResponseDto>>
            GetAllAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();

            return _mapper.Map<List<PaymentResponseDto>>(payments);
        }

        public async Task<PaymentResponseDto?> GetByIdAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);

            if (payment is null)
            {
                return null;
            }

            return _mapper.Map<PaymentResponseDto>(payment);
        }

        public async Task<PaymentResponseDto> CreateAsync(CreatePaymentRequestDto request)
        {
            await ValidatePaymentAsync(
                request.BookingId,
                request.ReceivedId,
                request.PaymentAmount,
                request.Status);

            var payment = _mapper.Map<Payment>(request);

            payment.PaymentDate = DateTime.Today;

            var createdPayment = await _paymentRepository.CreateAsync(payment);

            return _mapper.Map<PaymentResponseDto>(createdPayment);
        }

        public async Task<PaymentResponseDto?> UpdateAsync(int paymentId, UpdatePaymentRequestDto request)
        {
            var payment =await _paymentRepository.GetByIdAsync(paymentId);

            if (payment is null)
            {
                return null;
            }

            await ValidatePaymentAsync(
                request.BookingId,
                request.ReceivedId,
                request.PaymentAmount,
                request.Status,
                paymentId);

            _mapper.Map(request, payment);

            var updatedPayment =
                await _paymentRepository.UpdateAsync(payment);

            return _mapper.Map<PaymentResponseDto>(
                updatedPayment);
        }

        public async Task<bool> DeleteAsync(int paymentId)
        {
            return await _paymentRepository.DeleteAsync(
                paymentId);
        }


        // Business rules: 
        //1 . The booking must exist.
        //2 . The receiving employee must exist and be active.
        //3 . The payment amount must not exceed the remaining booking amount if the payment status is "Completed".
        //4. Employee must be active to receive the payment.
        //5 The amount of payment can not exceed the total amount of the booking if the payment status is "Completed".
        private async Task ValidatePaymentAsync(
            int bookingId,
            int receivedId,
            double paymentAmount,
            string status,
            int? excludedPaymentId = null)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            //1 . The booking must exist.

            if (booking is null)
            {
                throw new ArgumentException(
                    "Booking was not found.");
            }

            var employee = await _employeeRepository.GetByIdAsync(receivedId);

            //2 . The receiving employee must exist and be active.

            if (employee is null)
            {
                throw new ArgumentException(
                    "Receiving employee was not found.");
            }

            //4. Employee must be active to receive the payment.
            if (!employee.IsActive)
            {
                throw new ArgumentException(
                    "Receiving employee is inactive.");
            }

            //3 . The payment amount must not exceed the remaining booking amount if the payment status is "Completed".

            var payments = await _paymentRepository.GetByBookingIdAsync(bookingId);

            var completedPaymentAmount = payments
                .Where(payment =>
                    string.Equals(
                        payment.Status,
                        "Completed",
                        StringComparison.OrdinalIgnoreCase) &&
                    (!excludedPaymentId.HasValue ||
                     payment.PaymentId !=
                     excludedPaymentId.Value))
                .Sum(payment => payment.PaymentAmount);



            var isCompletedPayment =
                string.Equals(
                    status,
                    "Completed",
                    StringComparison.OrdinalIgnoreCase);

            //5 The amount of payment can not exceed the total amount of the booking if the payment status is "Completed".

            if (isCompletedPayment &&
                completedPaymentAmount + paymentAmount >
                booking.TotalAmount)
            {
                var remainingAmount = Math.Max(
                    0,
                    booking.TotalAmount -
                    completedPaymentAmount);

                throw new ArgumentException(
                    $"The payment exceeds the remaining booking amount. Only {remainingAmount} is remaining.");
            }
        }
    }
}