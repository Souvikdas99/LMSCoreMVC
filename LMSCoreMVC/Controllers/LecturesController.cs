using Microsoft.AspNetCore.Mvc;

namespace LMSCoreMVC.Controllers
{
    public class LecturesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
