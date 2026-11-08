namespace warehouse_api.DTOs.Customers
{
    public class CreateCustomerRequestDto
    {
        public string CustName { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string Mobile { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? GstNo { get; set; }
    }
}