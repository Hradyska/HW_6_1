using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter.HasValue)
        {
            query = query.Where(w => w.CatalogBrandId == brandFilter.Value);
        }

        if (typeFilter.HasValue)
        {
            query = query.Where(w => w.CatalogTypeId == typeFilter.Value);
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query.OrderBy(c => c.Name)
           .Include(i => i.CatalogBrand)
           .Include(i => i.CatalogType)
           .Skip(pageSize * pageIndex)
           .Take(pageSize)
           .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    // public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize)
    // {
    //    var totalItems = await _dbContext.CatalogItems
    //        .LongCountAsync();
    //    var itemsOnPage = await _dbContext.CatalogItems
    //        .Include(i => i.CatalogBrand)
    //        .Include(i => i.CatalogType)
    //        .OrderBy(c => c.Name)
    //        .Skip(pageSize * pageIndex)
    //        .Take(pageSize)
    //        .ToListAsync();
    //    return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    // }
    public async Task<GetItems<CatalogItem>> GetByIdAsync(int id)
    {
        var items = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(c => c.Id == id)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new GetItems<CatalogItem>() { Data = items };
    }

    public async Task<GetItems<CatalogItem>> GetByBrandAsync(int brandId)
    {
        var items = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(c => c.CatalogBrandId == brandId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new GetItems<CatalogItem>() { Data = items };
    }

    public async Task<GetItems<CatalogItem>> GetByTypeAsync(int typeId)
    {
        var items = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(c => c.CatalogTypeId == typeId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new GetItems<CatalogItem>() { Data = items };
    }

    public async Task<int?> Add(CatalogItem item)
    {
        var catalogItem = await _dbContext.AddAsync(item);

        await _dbContext.SaveChangesAsync();

        return catalogItem.Entity.Id;
    }

    public async Task<int?> Remove(int id)
    {
        var item = await _dbContext.CatalogItems.SingleAsync(c => c.Id == id);

        _dbContext.CatalogItems.Remove(item);
        _dbContext.SaveChanges();

        return item.Id;
    }

    public async Task<int?> Update(CatalogItem item)
    {
        var catalogItem = _dbContext.CatalogItems.Update(item);
        await _dbContext.SaveChangesAsync();
        return catalogItem.Entity.Id;
    }
}