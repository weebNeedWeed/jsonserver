using Microsoft.AspNetCore.Mvc;

namespace jsonserver.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
