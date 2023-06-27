using System;
using System.Threading.Tasks;
using Infrastructure.RateLimit.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Infrastructure.RateLimit.Middlewares;

public class RateLimittingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _cache;
    private readonly int _limit;
    private readonly TimeSpan _duration;

    public RateLimittingMiddleware(RequestDelegate next, IDistributedCache cache, int limit, TimeSpan duration)
    {
        _next = next;
        _cache = cache;
        _limit = limit;
        _duration = duration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var endpoint = context.GetEndpoint;
        var key = $"rate_limit:{endpoint}{ipAddress}";


        var cacheEntry = await _cache.GetStringAsync(key);
        if (cacheEntry == null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _duration
            };
            await _cache.SetStringAsync(key, "1", options);
        }
        else
        {
            var count = int.Parse(cacheEntry);
            if (count >= _limit)
            {
                context.Response.StatusCode = 429; // Too Many Requests
                return;
            }

            count++;
            await _cache.SetStringAsync(key, count.ToString());
        }

        await _next(context);
    }
}
