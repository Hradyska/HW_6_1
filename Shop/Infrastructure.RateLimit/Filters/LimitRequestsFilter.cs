using System;
using System.Threading.Tasks;
using Infrastructure.RateLimit.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;

namespace Infrastructure.RateLimit.Filters
{
    public class LimitRequestsFilter : ActionFilterAttribute
    {
        private readonly int _limit;
       // private readonly IRedisCacheConnectionService _connectionService;
        public LimitRequestsFilter(int limit)
        {
            _limit = limit;
        }
        
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var endpoint = context.HttpContext.GetEndpoint();
            var key = $"{endpoint}:{ipAddress}";

            try
            {
                Console.WriteLine("enter point in rateLimit filter");
                var redis = ConnectionMultiplexer.Connect("www.alevelwebsite.com:6380");
                var database = redis.GetDatabase();

                var currentCount = await database.StringIncrementAsync(key);
                Console.WriteLine($"database in rateLimit filter for {key}");
                if (currentCount == 1)
                {
                    await database.KeyExpireAsync(key, TimeSpan.FromMinutes(1));
                }

                if (currentCount > _limit)
                {
                    context.Result = new StatusCodeResult(429); // Too Many Requests
                    return;
                }
            }
            catch (Exception ex)
            {
                // Handle connection or Redis errors
                Console.WriteLine($"Exception rateLimit filter {ex.Message}");
                context.Result = new StatusCodeResult(500); // Internal Server Error
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}  

