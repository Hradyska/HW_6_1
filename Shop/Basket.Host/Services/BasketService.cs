using Basket.Host.Models.Responses;
using Basket.Host.Services.Interfaces;
using Infrastructure.RateLimit.Services.Interfaces;

namespace Basket.Host.Services
{
    public class BasketService : IBasketService
    {
        private readonly ILogger _logger;
        public BasketService(ILogger<BasketService> logger)
        {
            _logger = logger;
        }
        public Task<string> GetUserIdAsync(string userId)
        {
            _logger.LogInformation($"user ID is: {userId}");
            return Task.FromResult(userId);
        }
        public async Task LoggerAsync(string logg)
        {
            _logger.LogInformation(logg);
        }
    }
}
