using Basket.Host.Models.Requests;
using Basket.Host.Models.Responses;
using Basket.Host.Services.Interfaces;
using Basket.Host.Models.Dtos;
namespace Basket.Host.Services
{
    public class BasketService : IBasketService
    {
        public BasketService(
            ILogger<BasketService> logger)
        {
        }
        [HttpPost]
        [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
        public Task Add(AddItemRequest item, string userId)
        {
            BasketItemDto basketItem = new BasketItemDto()
            {
                Id = userId,
                ItemId = item.ItemId,
            };
            return Task.CompletedTask;
        }
    }
}
