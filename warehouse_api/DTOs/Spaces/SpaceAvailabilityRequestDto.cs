namespace warehouse_api.DTOs.Spaces
{
    public class SpaceAvailabilityRequestDto
    {
        public int WarehouseId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime TillDate { get; set; }

    }
}
