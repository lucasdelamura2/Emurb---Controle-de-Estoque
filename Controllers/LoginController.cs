using EmurbEstoque.Models;
using EmurbEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepository _repository;

        public LoginController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Senha)
        {
            Usuario usuario = _repository.ValidateCredentials(Email, Senha);

            if (usuario == null)
            {
                ViewBag.Error = "E-mail ou senha inválidos. Tente novamente.";
                return View("Index"); 
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.IdUsuario);
            
            if (usuario.FuncionarioId.HasValue)
            {
                HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}