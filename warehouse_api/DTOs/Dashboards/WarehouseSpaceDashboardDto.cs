namespace warehouse_api.DTOs.Dashboards
{
    public class WarehouseSpaceDashboardDto
    {
        public int SpaceId { get; set; }

        public string SpaceName { get; set; } = string.Empty;

        public int TotalAreaSqft { get; set; }

        public int BookedAreaSqft { get; set; }

        public int AvailableAreaSqft { get; set; }

        public int ActiveBookings { get; set; }

        public bool IsAvailable { get; set; }
    }
}
