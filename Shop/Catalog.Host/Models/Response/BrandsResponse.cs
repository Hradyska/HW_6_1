namespace Catalog.Host.Models.Response
{
    public class BrandsResponse<T>
    {
        public IEnumerable<T> Data { get; init; } = null!;
    }
}
