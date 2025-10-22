using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class LocalController : Controller
    {
        // Lista estática pública para simular a tabela Locais
        public static readonly List<Local> _locaisMem = new();
        private static int _nextId = 1;

        // GET: /Local/
        public IActionResult Index()
        {
            return View(_locaisMem.OrderBy(l => l.Nome).ToList());
        }

        // GET: /Local/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Local/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Local local)
        {
            if (!ModelState.IsValid)
            {
                return View(local);
            }
            local.IdLocal = _nextId++;
            _locaisMem.Add(local);
            return RedirectToAction(nameof(Index));
        }
    }
}