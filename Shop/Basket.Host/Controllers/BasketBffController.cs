using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Basket.Host.Services.Interfaces;
using Basket.Host.Services;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Basket.Host.Models.Responses;
using Infrastructure.RateLimit.Filters;
namespace Basket.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class BasketBffController : ControllerBase
    {
        private readonly ILogger<BasketBffController> _logger;
        private readonly IBasketService _basketService;

        public BasketBffController(
            ILogger<BasketBffController> logger,
            IBasketService catalogService)
        {
            _logger = logger;
            _basketService = catalogService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [LimitRequestsFilter(10)]
        public async Task<IActionResult> BasketLogger()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "sub");
            if (user == null)
            {
                await _basketService.LoggerAsync("Log in without authorization.");
                return Ok();
            }
            await _basketService.LoggerAsync($"Log in with Id: {user}");
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [LimitRequestsFilter(10)]
        public async Task<IActionResult> GetId()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var res = await _basketService.GetUserIdAsync(user);
            return Ok(res);
        }

    }
}

    
