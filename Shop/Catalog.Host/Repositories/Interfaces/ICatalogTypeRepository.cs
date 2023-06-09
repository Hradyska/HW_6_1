using Catalog.Host.Data.Entities;
using Catalog.Host.Data;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogTypeRepository
    {
        Task<GetTypes<CatalogType>> GetAsync();
        Task<int?> Add(string type);
        Task<int?> Remove(int id);
        Task<int?> Update(int id, string type);
    }
}
