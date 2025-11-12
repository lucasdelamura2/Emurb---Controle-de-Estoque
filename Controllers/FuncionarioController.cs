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

            int statusCode = _repository.Create(funcionario);
            if (statusCode == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else if (statusCode == 1)
            {
                ViewBag.Error = "Este funcionário (CPF/CNPJ) já está cadastrado.";
            }
            else if (statusCode == 3) 
            {
                ViewBag.Error = "O e-mail informado já está em uso por outra pessoa.";
            }
            else
            {
                ViewBag.Error = "Problemas no cadastro do funcionário. Verifique os dados ou contate o administrador.";
            }
            return View(funcionario);
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
            int statusCode = _repository.Update(dados);
            if (statusCode == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else if (statusCode == 3)
            {
                ViewBag.Error = "O e-mail informado já está em uso por outra pessoa.";
            }
            else
            {
                ViewBag.Error = "Problemas ao alterar o funcionário. Esse CPF/CNPJ já está sendo utilizado.";
            }
            return View(dados);
        }

        public IActionResult Delete(int id)
        {
            int statusCode = _repository.Delete(id);

            if (statusCode == 1) 
            {
                TempData["Error"] = "Este funcionário não pode ser excluído, pois está em uso.";
            }
            else if (statusCode == 2) 
            {
                TempData["Error"] = "Houve um problema ao excluir o funcionário.";
            }
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