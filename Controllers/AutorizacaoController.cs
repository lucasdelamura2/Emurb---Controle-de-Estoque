using EmurbEstoque.Models;
using EmurbEstoque.Repositories; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Controllers
{
    public class AutorizacaoController : Controller
    {
        private readonly IAutorizacaoRepository _autorizacaoRepository;
        private readonly IAutorizadoRepository _autorizadoRepository;
        private readonly ILocalRepository _localRepository;

        public AutorizacaoController(
            IAutorizacaoRepository autorizacaoRepository,
            IAutorizadoRepository autorizadoRepository,
            ILocalRepository localRepository)
        {
            _autorizacaoRepository = autorizacaoRepository;
            _autorizadoRepository = autorizadoRepository;
            _localRepository = localRepository;
        }
        private void PrepararViewBags()
        {
            ViewBag.ListaAutorizados = new SelectList(_autorizadoRepository.GetAll(), "IdAutorizado", "Funcao");
            ViewBag.ListaLocais = new SelectList(_localRepository.GetAll(), "IdLocal", "Nome");
        }
        public IActionResult Index()
        {
            var autorizacoes = _autorizacaoRepository.GetAll();
            ViewBag.NomesAutorizados = _autorizadoRepository.GetAll().ToDictionary(a => a.IdAutorizado, a => a.Funcao);
            ViewBag.NomesLocais = _localRepository.GetAll().ToDictionary(l => l.IdLocal, l => l.Nome);
            return View(autorizacoes);
        }
        public IActionResult Create()
        {
            PrepararViewBags();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Autorizacao autorizacao)
        {
            if (_autorizacaoRepository.Exists(autorizacao.AutorizadoId, autorizacao.LocalId))
            {
                ModelState.AddModelError("", "Esta combinação de Função e Local já existe.");
            }

            if (!ModelState.IsValid)
            {
                PrepararViewBags();
                return View(autorizacao);
            }

            _autorizacaoRepository.Create(autorizacao);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var autorizacao = _autorizacaoRepository.GetById(id);
            if (autorizacao == null) return NotFound();
            _autorizacaoRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var autorizacao = _autorizacaoRepository.GetById(id);
            if (autorizacao == null) return NotFound();

            PrepararViewBags(); 
            return View(autorizacao);
        }

        [HttpPost]
        public IActionResult Edit(int id, Autorizacao autorizacao)
        {
            autorizacao.IdAutoriza = id;
            if (!ModelState.IsValid)
            {
                PrepararViewBags();
                return View(autorizacao);
            }

            try
            {
                _autorizacaoRepository.Update(autorizacao);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex) 
            {
                ModelState.AddModelError("", ex.Message);
                PrepararViewBags();
                return View(autorizacao);
            }
        }
    }
}