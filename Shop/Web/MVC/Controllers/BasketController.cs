using Microsoft.AspNetCore.Mvc;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public IActionResult Index()
        {

            return View();
        }
    }
}
