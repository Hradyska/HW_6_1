namespace Catalog.Host.Data
{
    public class GetItems<T>
    {
        public IEnumerable<T> Data { get; init; } = null!;
    }
}
