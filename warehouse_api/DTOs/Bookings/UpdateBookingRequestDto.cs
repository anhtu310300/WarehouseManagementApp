namespace warehouse_api.DTOs.Bookings
{
    public class UpdateBookingRequestDto
    {
        public int CustomerId { get; set; }

        public int WarehouseId { get; set; }

        public int SpaceId { get; set; }

        public DateTime BookingFromDate { get; set; }

        public DateTime BookingTillDate { get; set; }

        public int SqftAreaNeeded { get; set; }

        public double TotalAmount { get; set; }

        public int BookingDoneBy { get; set; }
    }
}
