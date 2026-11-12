namespace warehouse_api.DTOs.Customers
{
    public class UpdateCustomerRequestDto
    {
        public string CustName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string GstNo { get; set; } = string.Empty;
    }
}
