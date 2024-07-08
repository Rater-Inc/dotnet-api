using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace Rater.Business.Configurations
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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

            bool success = false;
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string message = "";
            string stackTrace = string.Empty;

            if(exceptionType == typeof(UnauthorizedAccessException))
            {
                success = false;
                status = HttpStatusCode.Unauthorized;
                message = ex.Message;
                stackTrace = ex.StackTrace ?? string.Empty;
            }

            else if (exceptionType == typeof(InvalidOperationException))
            {
                success = false;
                status = HttpStatusCode.BadRequest;
                message = ex.Message;
                stackTrace = ex.StackTrace ?? string.Empty;
            }
            else if (exceptionType == typeof(ArgumentException))
            {
                success = false;
                status = HttpStatusCode.BadRequest;
                message = ex.Message;
                stackTrace = ex.StackTrace ?? string.Empty;
            }
            else if (exceptionType == typeof(Exception))
            {
                success = false;
                status = HttpStatusCode.BadRequest;
                message = ex.Message;
                stackTrace = ex.StackTrace ?? string.Empty;
            }

            _logger.LogError(ex, "An error occurred: {Message}, StackTrace: {StackTrace}", message, stackTrace);


            var exceptionResult = JsonSerializer.Serialize(new { success, status, message ,stackTrace});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
