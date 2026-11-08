namespace warehouse_api.DTOs.Spaces
{
    public class SpaceResponseDto
    {
        public int SpaceId { get; set; }

        public int WarehouseId { get; set; }

        public string SpaceName { get; set; } = string.Empty;

        public int AreaSqft { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
