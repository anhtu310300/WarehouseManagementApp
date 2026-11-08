namespace warehouse_api.DTOs.Bookings
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public int WarehouseId { get; set; }

        public int? SpaceId { get; set; }

        public DateTime BookingFromDate { get; set; }

        public DateTime BookingTillDate { get; set; }

        public DateTime BookingDate { get; set; }

        public int SqftAreaNeeded { get; set; }

        public double TotalAmount { get; set; }

        public int BookingDoneBy { get; set; }
    }
}