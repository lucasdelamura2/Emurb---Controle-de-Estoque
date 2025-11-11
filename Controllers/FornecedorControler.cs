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

                int statusCode = _repository.Create(fornecedor);

                if (statusCode == 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (statusCode == 1)
                {
                    ViewBag.Error = "Este fornecedor (CPF/CNPJ) já está cadastrado.";
                }
                else if (statusCode == 3)
                {
                    ViewBag.Error = "O e-mail informado já está em uso por outra pessoa.";
                }
                else 
                {
                    ViewBag.Error = "Problemas no cadastro. O CPF/CNPJ pode já existir como Funcionário, ou verifique os dados.";
                }
                return View(fornecedor);
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
                
                dados.IdFornecedor = id;
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
                    ViewBag.Error = "Problemas ao alterar o fornecedor. Verifique os dados.";
                }
                return View(dados);
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
                int statusCode = _repository.Delete(id);
                if (statusCode == 1)
                {
                    TempData["Error"] = "Este fornecedor não pode ser excluído, pois está vinculado a Ordens de Entrada.";
                }
                else if (statusCode == 2)
                {
                    TempData["Error"] = "Houve um problema ao excluir o fornecedor.";
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }