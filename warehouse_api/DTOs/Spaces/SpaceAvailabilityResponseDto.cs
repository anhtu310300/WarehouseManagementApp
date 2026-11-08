namespace warehouse_api.DTOs.Spaces
{
    public class SpaceAvailabilityResponseDto
    {
        public int SpaceId { get; set; }

        public int WarehouseId { get; set; }

        public string SpaceName { get; set; } = string.Empty;

        public int TotalAreaSqft { get; set; }

        public int BookedAreaSqft { get; set; }

        public int AvailableAreaSqft { get; set; }

        public bool IsAvailable { get; set; }
    }
}
