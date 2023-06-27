using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using Infrastructure.RateLimit.Middlewares;
namespace Infrastructure.RateLimit.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder, int limit, TimeSpan? duration)
        {
            return builder.UseMiddleware<RateLimittingMiddleware>(limit, duration);
        }
    }
}
