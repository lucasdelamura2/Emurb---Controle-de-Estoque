using EmurbEstoque.Models;
// Precisamos criar este ViewModel, assim como fizemos com OrdemEntrada
using EmurbEstoque.Models.ViewModels; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Controllers
{
    public class OrdemSaidaController : Controller
    {
        // --- SIMULAÇÃO DO BANCO ---
        public static readonly List<OrdemSaida> _ordensSaidaMem = new();
        public static readonly List<ItensOS> _itensSaidaMem = new();
        private static int _nextOrdemId = 1;
        private static int _nextItemId = 1;
        // --- FIM SIMULAÇÃO ---
        
        // Prepara os dropdowns para o formulário de Create
        private void PrepararViewBagsCreate()
        {
            // 1. Dropdown de Funcionários
            ViewBag.ListaFuncionarios = new SelectList(FuncionarioController._mem, "Id", "Nome");

            // 2. Dropdown de Autorizações (Função -> Local)
            var listaAutorizacoes = from a in AutorizacaoController._autorizacoesMem
                                    join au in AutorizadoController._autorizadosMem on a.AutorizadoId equals au.IdAutorizado
                                    join l in LocalController._locaisMem on a.LocalId equals l.IdLocal
                                    select new {
                                        Id = a.IdAutoriza,
                                        Descricao = $"{au.Funcao}  ->  {l.Nome}" // Texto que aparece no dropdown
                                    };
            
            ViewBag.ListaAutorizacoes = new SelectList(listaAutorizacoes, "Id", "Descricao");
        }

        // GET: /OrdemSaida/
        public IActionResult Index()
        {
            // Dicionários para exibir nomes na lista
            ViewBag.NomesFuncionarios = FuncionarioController._mem.ToDictionary(f => f.Id, f => f.Nome);
            
            var autorizacoesDesc = (from a in AutorizacaoController._autorizacoesMem
                                    join au in AutorizadoController._autorizadosMem on a.AutorizadoId equals au.IdAutorizado
                                    join l in LocalController._locaisMem on a.LocalId equals l.IdLocal
                                    select new { a.IdAutoriza, Desc = $"{au.Funcao} -> {l.Nome}" })
                                    .ToDictionary(k => k.IdAutoriza, v => v.Desc);
            ViewBag.Autorizacoes = autorizacoesDesc;

            return View(_ordensSaidaMem.OrderByDescending(o => o.DataSaida).ToList());
        }

        // --- ETAPA 1: CRIAR CABEÇALHO ---

        // GET: /OrdemSaida/Create
        public IActionResult Create()
        {
            PrepararViewBagsCreate();
            return View();
        }

        // POST: /OrdemSaida/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrdemSaida ordem)
        {
            if (!ModelState.IsValid)
            {
                PrepararViewBagsCreate();
                return View(ordem);
            }
            ordem.IdOrdSai = _nextOrdemId++;
            _ordensSaidaMem.Add(ordem);
            // Redireciona para a tela de Detalhes para adicionar os itens
            return RedirectToAction(nameof(Details), new { id = ordem.IdOrdSai });
        }

        // --- ETAPA 2: ADICIONAR ITENS ---

        // GET: /OrdemSaida/Details/5
        public IActionResult Details(int id)
        {
            var ordem = _ordensSaidaMem.FirstOrDefault(o => o.IdOrdSai == id);
            if (ordem == null) return NotFound();

            // *** LÓGICA DE CÁLCULO DE ESTOQUE DISPONÍVEL POR LOTE ***
            // 1. Pega todas as saídas já registradas
            var todasSaidasAgrupadas = _itensSaidaMem
                .GroupBy(i => i.LoteId)
                .Select(g => new { LoteId = g.Key, QtdSaida = g.Sum(i => i.Qtd) })
                .ToDictionary(k => k.LoteId, v => v.QtdSaida);

            // 2. Calcula o saldo atual de cada lote de entrada
            var lotesDisponiveis = OrdemEntradaController._lotesMem
                .Select(loteEntrada => new {
                    Lote = loteEntrada,
                    QtdSaida = todasSaidasAgrupadas.GetValueOrDefault(loteEntrada.IdLote, 0)
                })
                .Select(l => new {
                    l.Lote.IdLote,
                    l.Lote.ProdutoId,
                    EstoqueAtual = l.Lote.Qtd - l.QtdSaida // Saldo = Entrada - Saída
                })
                .Where(l => l.EstoqueAtual > 0) // Filtra apenas lotes com saldo
                .ToList();
            
            // Dicionário de Nomes de Produtos
            var nomesProdutos = ProdutoController._produtosMem.ToDictionary(p => p.IdProduto, p => p.Nome);

            // 3. Prepara o Dropdown de Lotes (com nome do produto e saldo)
            var listaLotesParaDropdown = lotesDisponiveis
                .Select(l => new {
                    Id = l.IdLote,
                    Descricao = $"Lote {l.IdLote} ({nomesProdutos.GetValueOrDefault(l.ProdutoId, "???")}) - Saldo: {l.EstoqueAtual}"
                });

            // Precisamos de um ViewModel novo (OrdemSaidaDetailsViewModel)
            // Vamos usar ViewBag por enquanto para simplificar
            ViewBag.Ordem = ordem;
            ViewBag.ItensNaOrdem = _itensSaidaMem.Where(i => i.OrdSaiId == id).ToList();
            
            // Dicionários para exibir nomes na tabela de itens
            var lotesDict = OrdemEntradaController._lotesMem.ToDictionary(l => l.IdLote);
            ViewBag.LotesInfo = lotesDict;
            ViewBag.NomesProdutos = nomesProdutos;

            ViewBag.ListaLotesDisponiveis = new SelectList(listaLotesParaDropdown, "Id", "Descricao");
            
            var formModel = new ItensOS { OrdSaiId = id };
            return View(formModel);
        }

        // POST: /OrdemSaida/AdicionarItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarItem(ItensOS item) // O nome 'item' deve bater no asp-for
        {
            // *** VALIDAÇÃO DE ESTOQUE (CRÍTICO) ***
            // (Aqui, precisaríamos recalcular o saldo do lote específico (item.LoteId)
            // e verificar se a 'item.Qtd' é <= ao saldo. Se não for, 
            // retorna para a View com um erro no ModelState)
            
            if (ModelState.IsValid)
            {
                item.IdItemOS = _nextItemId++;
                _itensSaidaMem.Add(item);
            }
            
            // Redireciona de volta para a tela de Detalhes
            return RedirectToAction(nameof(Details), new { id = item.OrdSaiId });
        }
    }
}