using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class HostsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
