namespace warehouse_api.Common.Responses
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> ErrorMessages { get; set; } = new();
        public bool IsSuccess { get; set; }
    }
}
