namespace warehouse_api.DTOs.Bookings
{
    public class BookingCalendarResponseDto
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public int WarehouseId { get; set; }

        public int? SpaceId { get; set; }

        public string? SpaceName { get; set; }

        public DateTime BookingFromDate { get; set; }

        public DateTime BookingTillDate { get; set; }

        public int SqftAreaNeeded { get; set; }

        public double TotalAmount { get; set; }
    }
}
