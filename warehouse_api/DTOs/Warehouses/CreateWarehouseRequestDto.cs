namespace warehouse_api.DTOs.Warehouses
{
    public class CreateWarehouseRequestDto
    {
        public string Title { get; set; } = string.Empty;

        public string Address1 { get; set; } = string.Empty;

        public string Address2 { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public int TotalArea { get; set; }

        public bool? IsColdStorageAvailable { get; set; }
    }
}
