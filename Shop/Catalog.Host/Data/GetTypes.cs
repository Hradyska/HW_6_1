namespace Catalog.Host.Data
{
    public class GetTypes<T>
    {
        public IEnumerable<T> Data { get; init; } = null!;
    }
}
