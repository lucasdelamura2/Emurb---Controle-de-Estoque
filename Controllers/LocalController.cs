using EmurbEstoque.Models;
using EmurbEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class LocalController : Controller
    {
        private readonly ILocalRepository _repository;
        public LocalController(ILocalRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var locais = _repository.GetAll();
            return View(locais);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Local local)
        {
            if (!ModelState.IsValid)
            {
                return View(local);
            }
            _repository.Create(local);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var local = _repository.GetById(id);
            if (local == null) return NotFound();
            return View(local);
        }
        [HttpPost]
        public IActionResult Edit(int id, Local local)
        {
            local.IdLocal = id;
            if (!ModelState.IsValid)
            {
                return View(local);
            }
            _repository.Update(local);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
             var local = _repository.GetById(id);
             if (local == null) return NotFound();
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}