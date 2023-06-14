using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public Task<int?> Add(CatalogItem item)
    {
        return ExecuteSafeAsync(() => _catalogItemRepository.Add(item));
    }

    public Task<int?> Remove(int id)
    {
        return ExecuteSafeAsync(() => _catalogItemRepository.Remove(id));
    }

    public Task<int?> Update(CatalogItem item)
    {
        return ExecuteSafeAsync(() => _catalogItemRepository.Update(item));
    }
}