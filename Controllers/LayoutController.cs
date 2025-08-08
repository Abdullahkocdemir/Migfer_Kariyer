using Microsoft.AspNetCore.Mvc;

namespace Migfer_Kariyer.Controllers
{
    public class LayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
