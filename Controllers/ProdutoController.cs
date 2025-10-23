using EmurbEstoque.Models;
using EmurbEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepository _repository;
        public ProdutoController(IProdutoRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var lista = _repository.GetAll();
            return View(lista); 
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return View(produto);
            }
            _repository.Create(produto);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var produto = _repository.GetById(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }
        [HttpPost]
        public IActionResult Edit(int id, Produto produto)
        {
            produto.IdProduto = id;

            if (!ModelState.IsValid)
            {
                return View(produto);
            }
            _repository.Update(produto);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var produto = _repository.GetById(id); 
            if (produto == null) 
            {
                return NotFound();
            }
            return View(produto); 
        }
    }
}