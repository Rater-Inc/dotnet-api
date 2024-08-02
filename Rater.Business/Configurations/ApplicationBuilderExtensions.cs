using Microsoft.AspNetCore.Builder;

namespace Rater.Business.Configurations
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            return applicationBuilder;
        }
    }
}
