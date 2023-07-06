using Basket.Host.Models.Responses;

namespace Basket.Host.Services.Interfaces
{
    public interface IBasketService
    {
        public Task<string> GetUserIdAsync(string userId);
        public Task LoggerAsync(string logg);
    }
}
