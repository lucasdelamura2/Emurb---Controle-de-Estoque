using EmurbEstoque.Models;
using EmurbEstoque.Repositories; 
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioRepository _repository;
        public FuncionarioController(IFuncionarioRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View(_repository.Read().OrderBy(f => f.IdFuncionario).ToList());
        }
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Funcionario funcionario)
        {
            if (!ModelState.IsValid)
            {
                return View(funcionario);
            }

            _repository.Create(funcionario);
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
        public IActionResult Edit(int id, Funcionario dados)
        {
            if (!ModelState.IsValid)
            {
                return View(dados);
            }

            dados.IdFuncionario = id;          
            _repository.Update(dados);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
        {
            var f = _repository.Read(id);
            if (f is null) 
            {
                return NotFound();
            }
            return View(f); 
        }
    }
}