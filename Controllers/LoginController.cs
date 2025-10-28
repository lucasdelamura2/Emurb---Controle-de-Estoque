using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Senha)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
