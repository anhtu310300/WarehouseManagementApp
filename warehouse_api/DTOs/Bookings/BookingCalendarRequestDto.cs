namespace warehouse_api.DTOs.Bookings
{
    public class BookingCalendarRequestDto
    {
        public int WarehouseId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime TillDate { get; set; }
    }
}
