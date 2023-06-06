using Catalog.Host.Data.Entities;
using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Repositories
{
    public class CatalogTypeRepository : ICatalogTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogTypeRepository> _logger;

        public CatalogTypeRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogTypeRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<PaginatedTypes<CatalogType>> GetByPageAsync(int pageIndex, int pageSize)
        {
            var totalItems = await _dbContext.CatalogTypes
                .LongCountAsync();

            var itemsOnPage = await _dbContext.CatalogTypes
                .OrderBy(c => c.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedTypes<CatalogType>() { TotalCount = totalItems, Data = itemsOnPage };
        }

        public async Task<int?> Add(string type)
        {
            var item = await _dbContext.AddAsync(new CatalogType
            {
                Type = type,
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<int?> Remove(int id)
        {
            var item = await _dbContext.CatalogTypes.SingleAsync(c => c.Id == id);
            _dbContext.CatalogTypes.Remove(item);
            await _dbContext.SaveChangesAsync();
            return item.Id;
        }

        public async Task<int?> Update(int id, string type)
        {
            var item = _dbContext.CatalogTypes.Update(new CatalogType
            {
                Id = id,
                Type = type,
            });
            await _dbContext.SaveChangesAsync();
            return item.Entity.Id;
        }
    }
}
