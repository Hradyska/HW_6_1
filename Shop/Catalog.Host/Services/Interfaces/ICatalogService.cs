using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);
    Task<BrandsResponse<CatalogBrandDto>> GetCatalogBrandsAsync();
    Task<TypesResponse<CatalogTypeDto>> GetCatalogTypesAsync();
    Task<GetItemsResponse<CatalogItemDto>> GetCatalogItemsByIdAsync(int id);
    Task<GetItemsResponse<CatalogItemDto>> GetCatalogItemsByBrandAsync(int brandId);
    Task<GetItemsResponse<CatalogItemDto>> GetCatalogItemsByTypeAsync(int typeId);
}