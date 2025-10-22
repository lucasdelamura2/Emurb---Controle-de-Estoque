using EmurbEstoque.Models;
using EmurbEstoque.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Controllers
{
    public class OrdemEntradaController : Controller
    {
        // --- SIMULAÇÃO DO BANCO DE DADOS ---
        // Acessa as listas estáticas dos outros controllers
        // (ProdutoController._produtosMem)
        // (FornecedorController._mem)
        
        public static readonly List<OrdemEntrada> _ordensMem = new();
        public static readonly List<Lote> _lotesMem = new();

        private static int _nextOrdemId = 1;
        private static int _nextLoteId = 1;
        // --- FIM DA SIMULAÇÃO ---

        // GET: /OrdemEntrada/
        // Lista todas as Ordens de Entrada já criadas
        public IActionResult Index()
        {
            // Dicionário para "joindar" o nome do fornecedor
            ViewBag.Fornecedores = FornecedorController._mem.ToDictionary(f => f.Id, f => f.Nome);
            return View(_ordensMem.OrderByDescending(o => o.DataEnt).ToList());
        }

        // --- ETAPA 1: CRIAR O CABEÇALHO ---

        // GET: /OrdemEntrada/Create
        public IActionResult Create()
        {
            // Prepara o dropdown de Fornecedores para a View
            ViewBag.Fornecedores = new SelectList(FornecedorController._mem, "Id", "Nome");
            return View();
        }

        // POST: /OrdemEntrada/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrdemEntrada ordem)
        {
            if (!ModelState.IsValid)
            {
                // Se der erro, recarrega o dropdown e exibe os erros
                ViewBag.Fornecedores = new SelectList(FornecedorController._mem, "Id", "Nome");
                return View(ordem);
            }

            ordem.IdOrdEnt = _nextOrdemId++;
            _ordensMem.Add(ordem);

            // FLUXO: Redireciona para a tela de Detalhes para adicionar os Lotes
            return RedirectToAction(nameof(Details), new { id = ordem.IdOrdEnt });
        }


        // --- ETAPA 2: ADICIONAR ITENS (LOTES) ---

        // GET: /OrdemEntrada/Details/5
        // Esta é a tela principal de lançamento
        public IActionResult Details(int id)
        {
            var ordem = _ordensMem.FirstOrDefault(o => o.IdOrdEnt == id);
            if (ordem == null) return NotFound();

            // Prepara o ViewModel complexo
            var viewModel = new OrdemEntradaDetailsViewModel();

            // 1. Dados da Ordem e Fornecedor
            viewModel.Ordem = ordem;
            var fornecedor = FornecedorController._mem.FirstOrDefault(f => f.Id == ordem.FornId);
            viewModel.NomeFornecedor = fornecedor?.Nome ?? "Fornecedor não encontrado";

            // 2. Lista de Lotes já adicionados nesta Ordem
            viewModel.ItensDaOrdem = _lotesMem.Where(l => l.OrdEntId == id).ToList();
            
            // Dicionário de Nomes de Produtos para a tabela
            ViewBag.NomesProdutos = ProdutoController._produtosMem.ToDictionary(p => p.IdProduto, p => p.Nome);

            // 3. Prepara o formulário para adicionar um Novo Lote
            viewModel.NovoLoteForm = new Lote { OrdEntId = id }; // Pré-popula o Id da Ordem
            
            // 4. Prepara o dropdown de Produtos
            viewModel.ListaProdutos = new SelectList(ProdutoController._produtosMem, "IdProduto", "Nome");

            return View(viewModel);
        }

        // POST: /OrdemEntrada/AdicionarLote
        // Este método é chamado pelo formulário DENTRO da página Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarLote(Lote NovoLoteForm) // O nome deve bater com o asp-for
        {
            if (ModelState.IsValid)
            {
                NovoLoteForm.IdLote = _nextLoteId++;
                _lotesMem.Add(NovoLoteForm);

                // AQUI VOCÊ ATUALIZARIA O ESTOQUE TOTAL NA TABELA PRODUTO
            }
            else
            {
                // Se o formulário do Lote for inválido, o ideal é recarregar a
                // página de Detalhes e exibir os erros.
                // Mas isso exige recarregar todo o ViewModel
                // Por simplicidade, vamos apenas redirecionar.
            }

            // FLUXO: Redireciona de volta para a mesma tela de Detalhes
            return RedirectToAction(nameof(Details), new { id = NovoLoteForm.OrdEntId });
        }
    }
}