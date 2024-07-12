using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Rater.Business.Configurations
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly Dictionary<Type, HttpStatusCode> _exceptionStatusCodeMapping;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;

            _exceptionStatusCodeMapping = new Dictionary<Type, HttpStatusCode>
            {
                { typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized },
                { typeof(InvalidOperationException), HttpStatusCode.BadRequest },
                { typeof(ArgumentException), HttpStatusCode.BadRequest }
            };

        }

        public async Task Invoke (HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) 
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context , Exception ex)
        {
            var exceptionType = ex.GetType();
            var statusCode = _exceptionStatusCodeMapping.ContainsKey(exceptionType)
                ? _exceptionStatusCodeMapping[exceptionType]
                : HttpStatusCode.InternalServerError;

            var response = new
            {
                success = false,
                status = statusCode,
                message = ex.Message,
                stackTrace = ex.StackTrace ?? string.Empty
            };

            _logger.LogError(ex, "An error occurred: {Message}, StackTrace: {StackTrace}", response.message, response.stackTrace);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
