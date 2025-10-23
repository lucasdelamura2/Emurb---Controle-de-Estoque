using EmurbEstoque.Models;
using EmurbEstoque.Models.ViewModels;
using EmurbEstoque.Repositories; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Controllers
{
    public class OrdemSaidaController : Controller
    {
        private readonly IOrdemSaidaRepository _ordemSaidaRepository;
        private readonly IItensOSRepository _itensOSRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IAutorizacaoRepository _autorizacaoRepository;
        private readonly IAutorizadoRepository _autorizadoRepository;
        private readonly ILocalRepository _localRepository;
        private readonly ILoteRepository _loteRepository; 
        private readonly IProdutoRepository _produtoRepository; 

        public OrdemSaidaController(
            IOrdemSaidaRepository ordemSaidaRepository,
            IItensOSRepository itensOSRepository,
            IFuncionarioRepository funcionarioRepository,
            IAutorizacaoRepository autorizacaoRepository,
            IAutorizadoRepository autorizadoRepository,
            ILocalRepository localRepository,
            ILoteRepository loteRepository,
            IProdutoRepository produtoRepository)
        {
            _ordemSaidaRepository = ordemSaidaRepository;
            _itensOSRepository = itensOSRepository;
            _funcionarioRepository = funcionarioRepository;
            _autorizacaoRepository = autorizacaoRepository;
            _autorizadoRepository = autorizadoRepository;
            _localRepository = localRepository;
            _loteRepository = loteRepository;
            _produtoRepository = produtoRepository;
        }
        private void PrepararViewBagsCreate()
        {
            ViewBag.ListaFuncionarios = new SelectList(_funcionarioRepository.Read(), "Id", "Nome");

            var listaAutorizacoes = from a in _autorizacaoRepository.GetAll()
                                    join au in _autorizadoRepository.GetAll() on a.AutorizadoId equals au.IdAutorizado
                                    join l in _localRepository.GetAll() on a.LocalId equals l.IdLocal
                                    select new {
                                        Id = a.IdAutoriza,
                                        Descricao = $"{au.Funcao}  ->  {l.Nome}"
                                    };
            ViewBag.ListaAutorizacoes = new SelectList(listaAutorizacoes, "Id", "Descricao");
        }
        public IActionResult Index()
        {
            var ordens = _ordemSaidaRepository.GetAll();
            ViewBag.NomesFuncionarios = _funcionarioRepository.Read().ToDictionary(f => f.Id, f => f.Nome);
            var autorizacoesDesc = (from a in _autorizacaoRepository.GetAll()
                                    join au in _autorizadoRepository.GetAll() on a.AutorizadoId equals au.IdAutorizado
                                    join l in _localRepository.GetAll() on a.LocalId equals l.IdLocal
                                    select new { a.IdAutoriza, Desc = $"{au.Funcao} -> {l.Nome}" })
                                    .ToDictionary(k => k.IdAutoriza, v => v.Desc);
            ViewBag.Autorizacoes = autorizacoesDesc;

            return View(ordens);
        }
        public IActionResult Create()
        {
            PrepararViewBagsCreate();
            return View();
        }
        [HttpPost]
        public IActionResult Create(OrdemSaida ordem)
        {
            if (!ModelState.IsValid)
            {
                PrepararViewBagsCreate();
                return View(ordem);
            }
            var ordemCriada = _ordemSaidaRepository.Create(ordem);
            return RedirectToAction(nameof(Details), new { id = ordemCriada.IdOrdSai });
        }
        private IActionResult PrepararDetailsView(int id, ItensOS? formModelComErro = null)
        {
            var ordem = _ordemSaidaRepository.GetById(id);
            if (ordem == null) return NotFound("Ordem de Saída não encontrada.");
            var todosItensSaida = _itensOSRepository.GetAll(); 
            var todosLotesEntrada = _loteRepository.GetAll(); 

            var saldosLotes = todosLotesEntrada
                .Select(loteEntrada => new {
                    Lote = loteEntrada,
                    QtdSaida = todosItensSaida
                                 .Where(itemSaida => itemSaida.LoteId == loteEntrada.IdLote)
                                 .Sum(itemSaida => itemSaida.Qtd)
                })
                .Select(l => new {
                    l.Lote.IdLote,
                    l.Lote.ProdutoId,
                    EstoqueAtual = l.Lote.Qtd - l.QtdSaida
                })
                .Where(l => l.EstoqueAtual > 0)
                .ToList();

            var nomesProdutos = _produtoRepository.GetAll().ToDictionary(p => p.IdProduto, p => p.Nome);

            var listaLotesParaDropdown = saldosLotes
                .Select(l => new {
                    Id = l.IdLote,
                    Descricao = $"Lote {l.IdLote} ({nomesProdutos.GetValueOrDefault(l.ProdutoId, "???")}) - Saldo: {l.EstoqueAtual}"
                });
            var funcionario = _funcionarioRepository.Read(ordem.FuncId);
            var autorizacao = _autorizacaoRepository.GetById(ordem.AutorizaId);
            var autorizado = autorizacao != null ? _autorizadoRepository.GetById(autorizacao.AutorizadoId) : null;
            var local = autorizacao != null ? _localRepository.GetById(autorizacao.LocalId) : null;
            var viewModel = new OrdemSaidaDetailsViewModel
            {
                Ordem = ordem,
                NomeFuncionario = funcionario?.Nome ?? "???",
                DescricaoAutorizacao = (autorizado != null && local != null) ? $"{autorizado.Funcao} -> {local.Nome}" : "???",
                ItensDaOrdem = _itensOSRepository.GetByOrdemSaidaId(id),
                NovoItemForm = formModelComErro ?? new ItensOS { OrdSaiId = id },
                ListaLotesDisponiveis = new SelectList(listaLotesParaDropdown, "Id", "Descricao")
            };
            ViewBag.LotesInfo = todosLotesEntrada.ToDictionary(l => l.IdLote);
            ViewBag.NomesProdutos = nomesProdutos;
            return View("Details", viewModel);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            return PrepararDetailsView(id);
        }
        [HttpPost]
        public IActionResult AdicionarItem(ItensOS NovoItemForm)
        {
            var todosItensSaida = _itensOSRepository.GetAll();
            var loteEntrada = _loteRepository.GetById(NovoItemForm.LoteId);
            int qtdJaSaiu = todosItensSaida
                              .Where(i => i.LoteId == NovoItemForm.LoteId)
                              .Sum(i => i.Qtd);
            int saldoDisponivel = (loteEntrada?.Qtd ?? 0) - qtdJaSaiu;

            if (NovoItemForm.Qtd > saldoDisponivel)
            {
                ModelState.AddModelError("NovoItemForm.Qtd", $"Quantidade indisponível. Saldo atual do lote: {saldoDisponivel}");
            }

            if (ModelState.IsValid)
            {
                _itensOSRepository.Create(NovoItemForm);
                return RedirectToAction(nameof(Details), new { id = NovoItemForm.OrdSaiId });
            }
            else
            {
                return PrepararDetailsView(NovoItemForm.OrdSaiId, NovoItemForm);
            }
        }
        [HttpPost] 
        public IActionResult RemoverItem(int idItemOS, int idOrdemSaida)
        {
            _itensOSRepository.Delete(idItemOS);
            return RedirectToAction(nameof(Details), new { id = idOrdemSaida });
        }
    }
}