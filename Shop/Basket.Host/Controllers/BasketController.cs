using Microsoft.AspNetCore.Authorization;
using System.Net;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Basket.Host.Services.Interfaces;
using Basket.Host.Models.Responses;
using Basket.Host.Models.Requests;

namespace Basket.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("basket.basketitem")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class BasketController : Controller
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketService _basketService;

        public BasketController(ILogger<BasketController> logger, IBasketService basketService)
        {
            _logger = logger;
            _basketService = basketService;
        }
        [HttpPost]
        [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add(AddItemRequest request)
        {
            var basketId = User.Claims.FirstOrDefault(u => u.Value == "sub")?.Value;
            var addItem = _basketService.Add(request, basketId);
            return Ok();

        }

    }
}
