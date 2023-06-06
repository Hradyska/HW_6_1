namespace Catalog.Host.Models.Response
{
    public class GetItemsResponse<T>
    {
        public IEnumerable<T> Data { get; init; } = null!;
    }
}
