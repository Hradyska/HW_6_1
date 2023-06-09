namespace Catalog.Host.Models.Response
{
    public class TypesResponse<T>
    {
        public IEnumerable<T> Data { get; init; } = null!;
    }
}
