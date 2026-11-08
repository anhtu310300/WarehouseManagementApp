namespace warehouse_api.DTOs.Customers
{
    public class CustomerResponseDto
    {
        public int CustomerId { get; set; }

        public string? CustName { get; set; }

        public string? Address { get; set; }

        public string? Mobile { get; set; }

        public string? Email { get; set; }

        public string? GstNo { get; set; }
    }
}
