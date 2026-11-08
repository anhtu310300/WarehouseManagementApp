namespace warehouse_api.DTOs.Employees
{
    public class EmployeeResponseDto
    {
        public int EmpId { get; set; }

        public string EmpName { get; set; } = string.Empty;

        public int WarehouseAssignedId { get; set; }

        public string MobileNo { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}

// what it will reponse after client request, it will not response password hash or password, only the information that is safe to share with the client.
