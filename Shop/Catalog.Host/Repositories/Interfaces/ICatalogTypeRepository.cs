using Catalog.Host.Data.Entities;
using Catalog.Host.Data;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogTypeRepository
    {
        Task<PaginatedTypes<CatalogType>> GetByPageAsync(int pageIndex, int pageSize);
        Task<int?> Add(string type);
        Task<int?> Remove(int id);
        Task<int?> Update(int id, string type);
    }
}
