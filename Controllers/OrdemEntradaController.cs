using EmurbEstoque.Models;
using EmurbEstoque.Models.ViewModels;
using EmurbEstoque.Repositories; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Controllers
{
    public class OrdemEntradaController : Controller
    {
        private readonly IOrdemEntradaRepository _ordemEntradaRepository;
        private readonly ILoteRepository _loteRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoRepository _produtoRepository;   
        public OrdemEntradaController(
            IOrdemEntradaRepository ordemEntradaRepository,
            ILoteRepository loteRepository,
            IFornecedorRepository fornecedorRepository,
            IProdutoRepository produtoRepository)
        {
            _ordemEntradaRepository = ordemEntradaRepository;
            _loteRepository = loteRepository;
            _fornecedorRepository = fornecedorRepository;
            _produtoRepository = produtoRepository;
        }
        public IActionResult Index()
        {
            var ordens = _ordemEntradaRepository.GetAll();
            
            ViewBag.Fornecedores = _fornecedorRepository.Read()
                                         .ToDictionary(f => f.IdFornecedor, f => f.Nome); 
            return View(ordens);
        }
        public IActionResult Create()
        {
            ViewBag.Fornecedores = new SelectList(_fornecedorRepository.Read(), "IdFornecedor", "Nome"); 
            return View();
        }
        [HttpPost]
        public IActionResult Create(OrdemEntrada ordem)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Fornecedores = new SelectList(_fornecedorRepository.Read(), "IdFornecedor", "Nome");
                return View(ordem);
            }
            var ordemCriada = _ordemEntradaRepository.Create(ordem);
            return RedirectToAction(nameof(Details), new { id = ordemCriada.IdOrdEnt });
        }
        private OrdemEntradaDetailsViewModel PrepararDetailsViewModel(int id, Lote? loteForm = null)
        {
            var ordem = _ordemEntradaRepository.GetById(id);
            if (ordem == null) return null; 

            var fornecedor = _fornecedorRepository.Read(ordem.IdFornecedor); 
            var lotesNaOrdem = _loteRepository.GetByOrdemEntradaId(id);
            var todosProdutos = _produtoRepository.GetAll(); 

            var viewModel = new OrdemEntradaDetailsViewModel
            {
                Ordem = ordem,
                NomeFornecedor = fornecedor?.Nome ?? "Fornecedor não encontrado",
                ItensDaOrdem = lotesNaOrdem,
                NovoLoteForm = loteForm ?? new Lote { OrdEntId = id },
                ListaProdutos = new SelectList(todosProdutos, "IdProduto", "Nome")
            };
            ViewBag.NomesProdutos = todosProdutos.ToDictionary(p => p.IdProduto, p => p.Nome);

            return viewModel;
        }

        public IActionResult Details(int id)
        {
            var viewModel = PrepararDetailsViewModel(id);
            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AdicionarLote(Lote NovoLoteForm) 
        {
            var ordem = _ordemEntradaRepository.GetById(NovoLoteForm.OrdEntId);
            if (ordem?.Status == "Concluída")
            {
                return RedirectToAction(nameof(Details), new { id = NovoLoteForm.OrdEntId });
            }

            if (ModelState.IsValid)
            {
                _loteRepository.Create(NovoLoteForm);
                return RedirectToAction(nameof(Details), new { id = NovoLoteForm.OrdEntId });
            }
            else
            {
                var viewModel = PrepararDetailsViewModel(NovoLoteForm.OrdEntId, NovoLoteForm);
                if (viewModel == null) return NotFound();
                
                return View("Details", viewModel);
            }
        }
        
        [HttpPost]
        public IActionResult RemoverLote(int idLote, int idOrdemEntrada)
        {
            var ordem = _ordemEntradaRepository.GetById(idOrdemEntrada);
             if (ordem?.Status == "Concluída")
            {
                return RedirectToAction(nameof(Details), new { id = idOrdemEntrada });
            }

            _loteRepository.Delete(idLote);
            return RedirectToAction(nameof(Details), new { id = idOrdemEntrada });
        }

        [HttpPost]
        public IActionResult Concluir(int id)
        {
            _ordemEntradaRepository.Concluir(id);
            return RedirectToAction(nameof(Index)); 
        }

        [HttpGet]
        public IActionResult EditarLote(int id) 
        {
            var lote = _loteRepository.GetById(id);
            if (lote == null)
            {
                return NotFound();
            }
            var ordem = _ordemEntradaRepository.GetById(lote.OrdEntId);
            if (ordem?.Status == "Concluída")
            {
                return RedirectToAction(nameof(Details), new { id = lote.OrdEntId });
            }
            ViewBag.ListaProdutos = new SelectList(_produtoRepository.GetAll(), "IdProduto", "Nome");
            return View("EditarLote", lote); 
        }

        [HttpPost]
        public IActionResult EditarLote(int id, Lote lote)
        {
            lote.IdLote = id; 
            
            if (!ModelState.IsValid)
            {
                ViewBag.ListaProdutos = new SelectList(_produtoRepository.GetAll(), "IdProduto", "Nome");
                return View("EditarLote", lote);
            }

            _loteRepository.Update(lote);
            
            return RedirectToAction(nameof(Details), new { id = lote.OrdEntId });
        }
    }
}