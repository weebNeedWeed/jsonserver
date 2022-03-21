using Microsoft.AspNetCore.Mvc.Filters;
using jsonserver.Web.Extensions;

namespace jsonserver.Web.Attributes
{
    public class CustomAuthorizeAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Session.Get<string>(key: "AccessToken");
            System.Console.WriteLine(token);
        }
    }
}
