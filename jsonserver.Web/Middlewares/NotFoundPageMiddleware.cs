using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace jsonserver.Web.Middlewares
{
    public class NotFoundPageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _path;

        public NotFoundPageMiddleware(RequestDelegate next, string path)
        {
            _next = next;
            _path = path;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            int statusCode = context.Response.StatusCode;

            string currPath = context.Request.Path;

            if(statusCode == 404 && !currPath.Contains("/api"))
            {
                context.Response.Redirect(_path);
            }
        }
    }
}
