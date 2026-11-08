namespace warehouse_api.DTOs.Employees
{
    public class UpdateEmployeeRequestDto
    {
        public string EmpName { get; set; } = string.Empty;

        public int WarehouseAssignedId { get; set; }

        public string MobileNo { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? Password { get; set; }

        public bool IsActive { get; set; }
    }
}

// same with create
