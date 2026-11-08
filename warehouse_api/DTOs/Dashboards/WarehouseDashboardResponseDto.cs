namespace warehouse_api.DTOs.Dashboards
{
    public class WarehouseDashboardResponseDto
    {
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } =
            string.Empty;

        public DateTime DashboardDate { get; set; }

        public int WarehouseAreaSqft { get; set; }

        public int TotalSpaces { get; set; }

        public int TotalSpaceAreaSqft { get; set; }

        public int BookedAreaSqft { get; set; }

        public int AvailableAreaSqft { get; set; }

        public int ActiveBookings { get; set; }

        public List<WarehouseSpaceDashboardDto> Spaces
        {
            get;
            set;
        } = new();
    }
}
