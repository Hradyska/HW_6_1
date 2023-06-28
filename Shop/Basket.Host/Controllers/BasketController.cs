using Microsoft.AspNetCore.Authorization;
using System.Net;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Basket.Host.Services.Interfaces;
using Basket.Host.Models.Responses;
using Basket.Host.Models.Requests;
using Infrastructure.RateLimit.Filters;

namespace Basket.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("basket.basketitem")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketItemService<int> _basketItemService;

        public BasketController(ILogger<BasketController> logger, IBasketItemService<int> basketService)
        {
            _logger = logger;
            _basketItemService = basketService;
        }
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [LimitRequestsFilter(10)]
        public async Task<IActionResult> Add( AddRequest<int> itemId)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            await _basketItemService.AddAsync(itemId, user);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetDataResponse<int>),(int)HttpStatusCode.OK)]
        [LimitRequestsFilter(10)]
        public async Task<IActionResult> Get()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var result = await _basketItemService.GetDataAsync(user);
            return Ok(result);
        }
    }
}
