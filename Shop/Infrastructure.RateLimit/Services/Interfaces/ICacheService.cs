using System.Threading.Tasks;

namespace Infrastructure.RateLimit.Services.Interfaces
{
    public interface ICacheService
    {
        Task SetAsync<T>(string key, T value);
        Task AddOrUpdateAsync<T>(string key, T value);
        Task<T> GetAsync<T>(string key);
    }
}
