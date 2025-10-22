using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class ProdutoController : Controller
    {
        // SIMULAÇÃO DO BANCO: Lista estática de Produtos
        public static readonly List<Produto> _produtosMem = new();
        private static int _nextId = 1;

        // GET: /Produto/
        public IActionResult Index()
        {
            return View(_produtosMem.OrderBy(p => p.Nome).ToList());
        }

        // GET: /Produto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return View(produto);
            }

            produto.IdProduto = _nextId++;
            _produtosMem.Add(produto);
            return RedirectToAction(nameof(Index));
        }
        
        // (Você pode adicionar Edit, Details e Delete depois)
    }
}