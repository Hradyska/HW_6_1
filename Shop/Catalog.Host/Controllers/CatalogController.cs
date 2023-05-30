using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private static readonly string[] Names = new[]
        {
        "Cheese", "Pasta", "Tomato", "Basilic", "Cream", "Wine", "Fish", "Coffee", "Chocolate", "Milk"
        };
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ILogger<CatalogController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCatalog")]
        public IEnumerable<Catalog> Get()
        {
            return Enumerable.Range(0, 9).Select(index => new Catalog
            {
                Name = Names[index],
                Price = Random.Shared.Next(10, 100)/10,
                Count = Random.Shared.Next(1, 20)
            })
            .ToArray();
        }
        [HttpPost(Name = "PostCatalog")]
        public Catalog Post(string name, int count, double price)
        {
            Catalog catalog = new Catalog();
            catalog.Name = name;
            catalog.Price = price;
            catalog.Count = count;
            return catalog;
        }
    }
}
