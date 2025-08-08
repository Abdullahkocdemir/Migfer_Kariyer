using Microsoft.AspNetCore.Mvc;

namespace Migfer_Kariyer.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
