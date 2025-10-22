using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Controllers
{
    public class AutorizacaoController : Controller
    {
        // Lista estática pública para simular a tabela Autorizacao
        public static readonly List<Autorizacao> _autorizacoesMem = new();
        private static int _nextId = 1;

        // Função interna para carregar os dropdowns
        private void PrepararViewBags()
        {
            // Busca dados das listas estáticas dos outros controllers
            ViewBag.ListaAutorizados = new SelectList(AutorizadoController._autorizadosMem, "IdAutorizado", "Funcao");
            ViewBag.ListaLocais = new SelectList(LocalController._locaisMem, "IdLocal", "Nome");
        }

        // GET: /Autorizacao/
        public IActionResult Index()
        {
            // Dicionários para exibir os nomes na lista (simulando JOINs)
            ViewBag.NomesAutorizados = AutorizadoController._autorizadosMem.ToDictionary(a => a.IdAutorizado, a => a.Funcao);
            ViewBag.NomesLocais = LocalController._locaisMem.ToDictionary(l => l.IdLocal, l => l.Nome);
            return View(_autorizacoesMem);
        }

        // GET: /Autorizacao/Create
        public IActionResult Create()
        {
            PrepararViewBags(); // Prepara os dropdowns
            return View();
        }

        // POST: /Autorizacao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Autorizacao autorizacao)
        {
            if (!ModelState.IsValid)
            {
                PrepararViewBags(); // Recarrega os dropdowns se der erro
                return View(autorizacao);
            }
            autorizacao.IdAutoriza = _nextId++;
            _autorizacoesMem.Add(autorizacao);
            return RedirectToAction(nameof(Index));
        }
    }
}