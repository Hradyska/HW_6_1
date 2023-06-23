namespace Catalog.Host.Data
{
    public class GetBrands<T>
    {
        public IEnumerable<T> Data { get; init; } = null!;
    }
}
