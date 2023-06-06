using Catalog.Host.Data.Entities;
using Catalog.Host.Data;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogBrandRepository
    {
        Task<PaginatedBrands<CatalogBrand>> GetByPageAsync(int pageIndex, int pageSize);
        Task<int?> Add(string brand);
        Task<int?> Remove(int id);
        Task<int?> Update(int id, string brand);
    }
}
