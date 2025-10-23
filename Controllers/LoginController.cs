using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
