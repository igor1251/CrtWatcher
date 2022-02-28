using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class CertificatesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
