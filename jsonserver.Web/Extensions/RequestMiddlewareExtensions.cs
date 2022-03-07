using Microsoft.AspNetCore.Builder;
using jsonserver.Web.Middlewares;

namespace jsonserver.Web.Extensions
{
    public static class RequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthenticationMiddleware>();
        }
    }
}
