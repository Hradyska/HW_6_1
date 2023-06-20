using Basket.Host.Models.Requests;

namespace Basket.Host.Services.Interfaces
{
    public interface IBasketService
    {
        public Task Add(AddItemRequest item, string userId);
    }
}
