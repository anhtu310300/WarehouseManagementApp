namespace warehouse_api.DTOs.Payments
{
    public class CreatePaymentRequestDto
    {
        public int BookingId { get; set; }

        public double PaymentAmount { get; set; }

        public string PaymentMode { get; set; } = string.Empty;

        public string PaymentNarration { get; set; }
            = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int ReceivedId { get; set; }

        // No PaymentDate property here, as it will be set to the current date and time when the payment is created.
    }
}