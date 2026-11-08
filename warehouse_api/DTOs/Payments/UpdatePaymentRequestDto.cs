namespace warehouse_api.DTOs.Payments
{
    public class UpdatePaymentRequestDto
    {
        public int BookingId { get; set; }

        public double PaymentAmount { get; set; }

        public string PaymentMode { get; set; } = string.Empty;

        public string PaymentNarration { get; set; }
            = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int ReceivedId { get; set; }
    }
}