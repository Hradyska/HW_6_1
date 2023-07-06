namespace Basket.Host.Models.Responses
{
    public class GetDataResponse<T>
    {
        public T? Data { get; set; } = default(T?);
    }
}
