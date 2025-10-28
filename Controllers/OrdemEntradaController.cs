using EmurbEstoque.Models;
using EmurbEstoque.Models.ViewModels;
using EmurbEstoque.Repositories; // Importante
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
                                     .ToDictionary(f => f.IdPessoa, f => f.Nome);
            return View(ordens);
        }
        public IActionResult Create()
        {
            // Prepara o dropdown de Fornecedores
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
        public IActionResult Details(int id)
        {
            var ordem = _ordemEntradaRepository.GetById(id);
            if (ordem == null) return NotFound();

            var fornecedor = _fornecedorRepository.Read(ordem.IdFornecedor);
            var lotesNaOrdem = _loteRepository.GetByOrdemEntradaId(id);
            var todosProdutos = _produtoRepository.GetAll(); 

            var viewModel = new OrdemEntradaDetailsViewModel
            {
                Ordem = ordem,
                NomeFornecedor = fornecedor?.Nome ?? "Fornecedor não encontrado",
                ItensDaOrdem = lotesNaOrdem,
                NovoLoteForm = new Lote { OrdEntId = id },
                ListaProdutos = new SelectList(todosProdutos, "IdProduto", "Nome")
            };
            ViewBag.NomesProdutos = todosProdutos.ToDictionary(p => p.IdProduto, p => p.Nome);

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AdicionarLote(Lote NovoLoteForm) 
        {
            if (ModelState.IsValid)
            {
                _loteRepository.Create(NovoLoteForm);
            }
            else
            {
                 return RedirectToAction(nameof(Details), new { id = NovoLoteForm.OrdEntId });
            }
            return RedirectToAction(nameof(Details), new { id = NovoLoteForm.OrdEntId });
        }
    }
}