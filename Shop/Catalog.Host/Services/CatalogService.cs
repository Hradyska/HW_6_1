#pragma warning disable CS8603 // Possible null reference return.
using AutoMapper;
using Catalog.Host.Configurations;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Catalog.Host.Models.Enums;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly ICatalogBrandRepository _catalogBrandRepository;
    private readonly ICatalogTypeRepository _catalogTypeRepository;
    private readonly IMapper _mapper;

    public CatalogService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        ICatalogBrandRepository catalogBrandRepository,
        ICatalogTypeRepository catalogTypeRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _catalogBrandRepository = catalogBrandRepository;
        _catalogTypeRepository = catalogTypeRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters)
    {
        return await ExecuteSafeAsync(async () =>
        {
            int? brandFilter = null;
            int? typeFilter = null;

            if (filters != null)
            {
                if (filters.TryGetValue(CatalogTypeFilter.Brand, out var brand))
                {
                    brandFilter = brand;
                }

                if (filters.TryGetValue(CatalogTypeFilter.Type, out var type))
                {
                    typeFilter = type;
                }
            }

            var result = await _catalogItemRepository.GetByPageAsync(pageIndex, pageSize, brandFilter, typeFilter);
            if (result == null)
            {
                return null;
            }

            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<GetItemsResponse<CatalogItemDto>> GetCatalogItemsByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }

            return new GetItemsResponse<CatalogItemDto>()
            {
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
            };
        });
    }

    public async Task<GetItemsResponse<CatalogItemDto>> GetCatalogItemsByBrandAsync(int brandId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetByBrandAsync(brandId);
            if (result == null)
            {
                return null;
            }

            return new GetItemsResponse<CatalogItemDto>()
            {
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
            };
        });
    }

    public async Task<GetItemsResponse<CatalogItemDto>> GetCatalogItemsByTypeAsync(int typeId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetByTypeAsync(typeId);
            if (result == null)
            {
                return null;
            }

            return new GetItemsResponse<CatalogItemDto>()
            {
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
            };
        });
    }

    public async Task<BrandsResponse<CatalogBrandDto>> GetCatalogBrandsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogBrandRepository.GetAsync();
            if (result == null)
            {
                return null;
            }

            return new BrandsResponse<CatalogBrandDto>()
            {
                Data = result.Data.Select(s => _mapper.Map<CatalogBrandDto>(s)).ToList(),
            };
        });
    }

    public async Task<TypesResponse<CatalogTypeDto>> GetCatalogTypesAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogTypeRepository.GetAsync();
            if (result == null)
            {
                return null;
            }

            return new TypesResponse<CatalogTypeDto>()
            {
                Data = result.Data.Select(s => _mapper.Map<CatalogTypeDto>(s)).ToList(),
            };
        });
    }
}