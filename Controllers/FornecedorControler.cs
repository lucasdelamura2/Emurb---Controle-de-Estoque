using EmurbEstoque.Models;
using EmurbEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueWeb.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly IFornecedorRepository _repository;
        public FornecedorController(IFornecedorRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var lista = _repository.Read();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Fornecedor fornecedor)
        {
            if (!ModelState.IsValid)
            {
                return View(fornecedor);
            }
            _repository.Create(fornecedor);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var f = _repository.Read(id);
            if (f is null) return NotFound();
            return View(f);
        }

        [HttpPost]
        public IActionResult Edit(int id, Fornecedor dados)
        {
            if (!ModelState.IsValid)
            {
                return View(dados);
            }
            dados.Id = id; // Garante o ID correto vindo da URL
            _repository.Update(dados);
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult Details(int id)
        {
            var f = _repository.Read(id);
            if (f is null) return NotFound();
            return View(f);
        }

        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}