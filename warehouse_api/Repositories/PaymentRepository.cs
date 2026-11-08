using Microsoft.EntityFrameworkCore;
using warehouse_api.Data;
using warehouse_api.Domain;
using warehouse_api.Repositories.Interfaces;

namespace warehouse_api.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly WarehouseDbContext _context;

        public PaymentRepository(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            return await _context.Payments.OrderBy(payment => payment.PaymentId).ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int paymentId)
        {
            return await _context.Payments.FindAsync(paymentId);
        }

        public async Task<List<Payment>> GetByBookingIdAsync(int bookingId)
        {
            return await _context.Payments.Where(payment =>payment.BookingId == bookingId).OrderBy(payment => payment.PaymentId).ToListAsync();
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);

            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);

            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<bool> DeleteAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);

            if (payment is null)
            {
                return false;
            }

            _context.Payments.Remove(payment);

            await _context.SaveChangesAsync();

            return true;
        } /// No isActive property in Payment entity, so we can delete the payment record from the database.
    }
}