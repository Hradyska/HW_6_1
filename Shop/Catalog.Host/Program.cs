using Catalog.Host.Data;
using Catalog.Host.Repositories;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using Catalog.Host.Configurations;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Infrastructure.RateLimit.Services.Interfaces;
using Infrastructure.RateLimit.Services;
using Infrastructure.RateLimit.Extensions;
using Infrastructure.RateLimit.Configurations;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
             .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "eShop- Catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog Service HTTP API"
                });

                var authority = configuration["Authorization:Authority"];
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                            TokenUrl = new Uri($"{authority}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                 { "mvc", "website" },
                                 { "catalog.catalogitem", "catalog.catalogitem" },
                                 { "catalog.catalogbrand", "catalog.catalogbrand" },
                                 { "catalog.catalogtype", "catalog.catalogtype" }
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            builder.AddConfiguration();
            builder.Services.Configure<RedisConfig>(
                builder.Configuration.GetSection("Redis"));

            builder.AddConfiguration();
            builder.Services.Configure<CatalogConfig>(configuration);
            builder.Services.AddAuthorization(configuration);

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddTransient<IJsonSerializer, JsonSerializer>();
            builder.Services.AddTransient<IDistributedCache, MemoryDistributedCache>();
            builder.Services.AddTransient<IRedisCacheConnectionService, RedisCacheConnectionService>();
            builder.Services.AddTransient<ICacheService, CacheService>();
            builder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
            builder.Services.AddTransient<ICatalogService, CatalogService>();
            builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();
            builder.Services.AddTransient<ICatalogBrandRepository, CatalogBrandRepository>();
            builder.Services.AddTransient<ICatalogBrandService, CatalogBrandService>();
            builder.Services.AddTransient<ICatalogTypeRepository, CatalogTypeRepository>();
            builder.Services.AddTransient<ICatalogTypeService, CatalogTypeService>();
            builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));
            builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            var app = builder.Build();

            app.UseSwagger()
                .UseSwaggerUI(setup =>
                {
                    setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Catalog.API V1");
                    setup.OAuthClientId("catalogswaggerui");
                    setup.OAuthAppName("Catalog Swagger UI");
                });

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiting(10, TimeSpan.FromMinutes(1));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            CreateDbIfNotExists(app);
            app.Run();
            IConfiguration GetConfiguration()
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

                return builder.Build();
            }

            void CreateDbIfNotExists(IHost host)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();

                        DbInitializer.Initialize(context).Wait();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred creating the DB.");
                    }
                }
            }
        }
    }
}