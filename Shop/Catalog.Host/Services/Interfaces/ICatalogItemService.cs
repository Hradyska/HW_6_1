using Catalog.Host.Data.Entities;
namespace Catalog.Host.Services.Interfaces;

public interface ICatalogItemService
{
    Task<int?> Add(CatalogItem item);
    Task<int?> Remove(int id);
    Task<int?> Update(CatalogItem item);
}