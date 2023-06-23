using Catalog.Host.Data.Entities;
using Catalog.Host.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CatalogBrand> CatalogBrands { get; set; } = null!;
        public DbSet<CatalogItem> CatalogItems { get; set; } = null!;
        public DbSet<CatalogType> CatalogTypes { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
           modelBuilder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
           modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        }
    }
}
