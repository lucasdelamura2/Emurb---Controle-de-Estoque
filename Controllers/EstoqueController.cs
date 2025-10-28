using EmurbEstoque.Models;
using EmurbEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq; 

namespace EmurbEstoque.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IProdutoRepository _produtoRepository;
        public EstoqueController(IEstoqueRepository estoqueRepository, IProdutoRepository produtoRepository)
        {
            _estoqueRepository = estoqueRepository;
            _produtoRepository = produtoRepository;
        }
        public IActionResult Index()
        {
            var estoqueAtual = _estoqueRepository.GetAll() ?? new List<Estoque>(); // evita null
            var produtos = _produtoRepository.GetAll() ?? new List<Produto>(); // evita null

            var viewModel = (from est in estoqueAtual
                            join prod in produtos on est.ProdutoId equals prod.IdProduto
                            orderby prod.Nome
                            select new EmurbEstoque.Models.ViewModels.EstoqueViewModel
                            {
                                NomeProduto = prod.Nome,
                                Quantidade = est.QuantidadeAtual
                            }).ToList();
            return View(viewModel ?? new List<EmurbEstoque.Models.ViewModels.EstoqueViewModel>());
        }
    }
}