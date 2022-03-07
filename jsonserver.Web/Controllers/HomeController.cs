using jsonserver.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace jsonserver.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            string userName = HttpContext.Session.Get<string>("UserName");
            ViewData["UserName"] = userName;
            return View();
        }
    }
}
