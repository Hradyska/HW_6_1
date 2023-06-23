using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<BasketService> _logger;

        public BasketService(IHttpClientService httpClient, ILogger<BasketService> logger, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
        }
        public async Task<string> BasketLogger(string message)
        {
            var result = await _httpClient.SendAsync<string, string>($"{_settings.Value.BasketUrl}/basketlogger", HttpMethod.Post, message);

            return result;
        }

        public async Task<string> BasketLoggerId()
        {
            var result = await _httpClient.SendAsync<string, object?>($"{_settings.Value.BasketUrl}/basketloggerid", HttpMethod.Post, null);
            return result.ToString();
        }
    }
}
