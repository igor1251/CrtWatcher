using Microsoft.AspNetCore.Mvc;

namespace WA4D0GWebPanel.Controllers
{
    public class SubjectsController : Controller
    {
        public IActionResult SubjectsList()
        {
            return View();
        }
    }
}
