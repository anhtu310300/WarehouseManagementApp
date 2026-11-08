namespace warehouse_api.DTOs.Auth
{
    public class LoginResponseDto
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
