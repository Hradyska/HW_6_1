using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Basket.Host.Services.Interfaces;
using Basket.Host.Services;

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
        public async Task<IActionResult> BasketLogger(string testMessage)
        {
            _logger.LogInformation($"Logg from method {nameof(BasketLogger)}: {testMessage}");
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> BasketLoggerId()
        {
            var basketLogg = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            _logger.LogInformation($"Logg from method {nameof(BasketLoggerId)}: userId = {basketLogg!} ");
            return Ok(basketLogg);
        }

    }
}

    
