namespace warehouse_api.DTOs.Employees
{
    public class CreateEmployeeRequestDto 
    {
        public string EmpName { get; set; } = string.Empty;

        public int WarehouseAssignedId { get; set; }

        public string MobileNo { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}


// wrap domain models with DTOs to avoid exposing sensitive information like password hashes and to provide a clear contract for API requests and responses.