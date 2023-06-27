using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter);

    // Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize);
    Task<GetItems<CatalogItem>> GetByIdAsync(int id);
    Task<GetItems<CatalogItem>> GetByBrandAsync(int brandId);
    Task<GetItems<CatalogItem>> GetByTypeAsync(int typeId);
    Task<int?> Add(CatalogItem item);
    Task<int?> Remove(int id);
    Task<int?> Update(CatalogItem item);
}