using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
