using Catalog.Host.Data.Entities;
using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public class CatalogBrandRepository : ICatalogBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogBrandRepository> _logger;

        public CatalogBrandRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogBrandRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<GetBrands<CatalogBrand>> GetAsync()
        {
            var brands = await _dbContext.CatalogBrands.ToListAsync();
            return new GetBrands<CatalogBrand>() { Data = brands };
        }

        public async Task<int?> Add(string brand)
        {
            var item = await _dbContext.AddAsync(new CatalogBrand
            {
                Brand = brand,
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<int?> Remove(int id)
        {
            var item = await _dbContext.CatalogBrands.SingleAsync(c => c.Id == id);
            _dbContext.CatalogBrands.Remove(item);
            await _dbContext.SaveChangesAsync();
            return item.Id;
        }

        public async Task<int?> Update(int id, string brand)
        {
            var item = _dbContext.CatalogBrands.Update(new CatalogBrand
            {
                Id = id,
                Brand = brand,
            });
            await _dbContext.SaveChangesAsync();
            return item.Entity.Id;
        }
    }
}
