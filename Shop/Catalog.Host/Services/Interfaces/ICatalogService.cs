using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);
    Task<PaginatedBrandsResponse<CatalogBrandDto>> GetCatalogBrandsAsync(int pageSize, int pageIndex);
    Task<PaginatedTypesResponse<CatalogTypeDto>> GetCatalogTypesAsync(int pageSize, int pageIndex);
    Task<GetItemsResponse<CatalogItemDto>> GetCatalogItemsByIdAsync(int id);
    Task<GetItemsResponse<CatalogItemDto>> GetCatalogItemsByBrandAsync(int brandId);
    Task<GetItemsResponse<CatalogItemDto>> GetCatalogItemsByTypeAsync(int typeId);
}