using Microsoft.AspNetCore.Mvc;

namespace WA4D0GWebPanel.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult SettingsEditor()
        {
            return View();
        }
    }
}
