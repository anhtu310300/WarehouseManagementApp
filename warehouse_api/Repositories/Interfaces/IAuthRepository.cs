using warehouse_api.Domain;

namespace warehouse_api.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<Employee?> GetByUserNameAsync(string userName);
    }
}
