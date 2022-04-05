using jsonserver.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace jsonserver.Web.Attributes
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string accessToken = context.HttpContext.Session.Get<string>(key: "AccessToken");
            string userName = context.HttpContext.Session.Get<string>(key: "UserName");

            // Get current path to pass to "ReturnUrl" query variable
            // user will be redirected to ReturnUrl after logging in
            string currentPath = context.HttpContext.Request.Path;

            if (accessToken == null || userName == null)
            {
                context.HttpContext.Response.Redirect($"/Account/Login?ReturnUrl={currentPath}");
            }
        }
    }
}
