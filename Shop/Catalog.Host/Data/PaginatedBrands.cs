namespace Catalog.Host.Data
{
    public class PaginatedBrands<T>
    {
        public long TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}
