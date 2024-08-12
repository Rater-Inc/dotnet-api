using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Rater.API
{
    public class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly ILogger<AuthorizationMiddlewareResultHandler> _logger;
        private readonly Microsoft.AspNetCore.Authorization.Policy.AuthorizationMiddlewareResultHandler _defaultHandler = new();
        public AuthorizationMiddlewareResultHandler(ILogger<AuthorizationMiddlewareResultHandler> logger)
        {
            _logger = logger;
        }
        public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Forbidden || !authorizeResult.Succeeded)
            {
                context.Response.StatusCode = 401;
                var response = new
                {
                    success = false,
                    status = 401,
                    message = "Unauthorized for this space"
                };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                _logger.LogInformation("401 failed authentication");
                return;
            }

            // If authorized, continue with the request pipeline
            await next(context);
        }
    }
}
