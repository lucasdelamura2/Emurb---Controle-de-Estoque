using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class AutorizadoController : Controller
    {
        // Lista estática pública para simular a tabela Autorizados
        public static readonly List<Autorizado> _autorizadosMem = new();
        private static int _nextId = 1;

        // GET: /Autorizado/
        public IActionResult Index()
        {
            return View(_autorizadosMem.OrderBy(a => a.Funcao).ToList());
        }

        // GET: /Autorizado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Autorizado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Autorizado autorizado)
        {
            if (!ModelState.IsValid)
            {
                return View(autorizado);
            }
            autorizado.IdAutorizado = _nextId++;
            _autorizadosMem.Add(autorizado);
            return RedirectToAction(nameof(Index));
        }
    }
}