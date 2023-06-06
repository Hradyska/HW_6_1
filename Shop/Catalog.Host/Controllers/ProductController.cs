using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private static readonly string[] Names = new[]
        {
        "Cheese", "Pasta", "Tomato", "Basilic", "Cream", "Wine", "Fish", "Coffee", "Chocolate", "Milk"
        };
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCatalog")]
        public IEnumerable<Product> Get()
        {
            return Enumerable.Range(0, 9).Select(index => new Product
            {
                Name = Names[index],
                Price = Random.Shared.Next(10, 100) / 10,
                Count = Random.Shared.Next(1, 20)
            })
            .ToArray();
        }

        [HttpPost(Name = "PostCatalog")]
        public Product Post(string name, int count, double price)
        {
            Product catalog = new Product();
            catalog.Name = name;
            catalog.Price = price;
            catalog.Count = count;
            return catalog;
        }
    }
}
