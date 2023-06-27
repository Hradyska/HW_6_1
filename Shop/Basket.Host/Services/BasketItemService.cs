using Basket.Host.Models.Requests;
using Basket.Host.Models.Responses;
using Basket.Host.Services.Interfaces;
using Basket.Host.Models.Dtos;
using Infrastructure.RateLimit.Services.Interfaces;

namespace Basket.Host.Services
{
    public class BasketItemService<T> : IBasketItemService<T>
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;
        public BasketItemService(ICacheService cacheService,
            ILogger<BasketItemService<T>> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }
       
        public async Task AddAsync(AddRequest<T> addItem, string key)
        {
            try
            {
                await _cacheService.AddOrUpdateAsync<AddRequest<T>>(key, addItem);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
        }

        public async Task<GetDataResponse<T>> GetDataAsync(string key)
        {
            var result = await _cacheService.GetAsync< GetDataResponse< T >>(key);
            _logger.LogInformation($"Got data for {key}: {result.Data?.ToString()}");
            return result;
        }
    }
}
