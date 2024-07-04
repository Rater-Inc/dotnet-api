using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;

namespace Rater.Business.Configurations
{
    public static class RateLimiterConfiguration
    {
        public static IServiceCollection AddApplicationRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
                {
                    options.Window = TimeSpan.FromSeconds(10);
                    options.PermitLimit = 3;
                    options.QueueLimit = 1;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
            });

            return services;
        }
    }
}
