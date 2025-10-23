using EmurbEstoque.Models;
using EmurbEstoque.Repositories; 
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class AutorizadoController : Controller
    {
        private readonly IAutorizadoRepository _repository;
        public AutorizadoController(IAutorizadoRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var autorizados = _repository.GetAll();
            return View(autorizados);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Autorizado autorizado)
        {
            if (!ModelState.IsValid)
            {
                return View(autorizado);
            }
            _repository.Create(autorizado); 
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var autorizado = _repository.GetById(id);
            if (autorizado == null) return NotFound();
            return View(autorizado);
        }
        [HttpPost]
        public IActionResult Edit(int id, Autorizado autorizado)
        {
            autorizado.IdAutorizado = id;
            if (!ModelState.IsValid)
            {
                return View(autorizado);
            }
            _repository.Update(autorizado);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var autorizado = _repository.GetById(id);
            if (autorizado == null) return NotFound();
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}