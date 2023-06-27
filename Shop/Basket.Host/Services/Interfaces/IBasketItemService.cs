using Basket.Host.Models.Requests;
using Basket.Host.Models.Responses;

namespace Basket.Host.Services.Interfaces
{
    public interface IBasketItemService<T>
    {
        public Task AddAsync(AddRequest<T> addItem, string userId);
        public Task<GetDataResponse<T>> GetDataAsync(string key);
    }
}
