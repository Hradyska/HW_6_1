using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basket.Host.Models.Responses;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;
using Infrastructure.RateLimit.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Basket.UnitTests.Services
{
    public class BasketServiceTest
    {
        private readonly IBasketService _basketService;

        private readonly Mock<ILogger<BasketService>> _logger;

        public BasketServiceTest()
        {
            _logger = new Mock<ILogger<BasketService>>();
            _basketService = new BasketService(_logger.Object);
        }

        [Fact]
        public async Task GetUserIdAsync_Success()
        {
            var testRequest = "1";
            var testResponse = "1";

            var result = await _basketService.GetUserIdAsync(testRequest);
            result.Should().Be(testResponse);
        }

        [Fact]
        public async Task GetUserIdAsync_Failed()
        {
            var testRequest = "100000";

            var result = await _basketService.GetUserIdAsync(testRequest);
            result.Should().Be(null);
        }
    }
}
