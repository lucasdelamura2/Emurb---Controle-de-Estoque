using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
