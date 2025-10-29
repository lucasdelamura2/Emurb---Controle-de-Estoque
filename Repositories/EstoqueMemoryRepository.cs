using EmurbEstoque.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmurbEstoque.Repositories
{
    public class EstoqueMemoryRepository : IEstoqueRepository
    {
        private readonly IProdutoRepository _produtoRepo;
        private readonly ILoteRepository _loteRepo;   
        private readonly IItensOSRepository _itensOSRepo;  
        public EstoqueMemoryRepository(
            IProdutoRepository produtoRepository,
            ILoteRepository loteRepository,
            IItensOSRepository itensOSRepository)
        {
            _produtoRepo = produtoRepository;
            _loteRepo = loteRepository;
            _itensOSRepo = itensOSRepository;
        }

        public List<Estoque> GetEstoqueConsolidado()
        {
            var todosProdutos = _produtoRepo.GetAll();
            var todasEntradas = _loteRepo.GetAll();
            var todasSaidas = _itensOSRepo.GetAll();

            var estoqueConsolidado = new List<Estoque>();

            foreach (var produto in todosProdutos)
            {
                int totalEntrada = todasEntradas
                    .Where(l => l.ProdutoId == produto.IdProduto)
                    .Sum(l => l.Qtd);
                var lotesDoProdutoIds = todasEntradas
                    .Where(l => l.ProdutoId == produto.IdProduto)
                    .Select(l => l.IdLote)
                    .ToList();
                int totalSaida = todasSaidas
                    .Where(s => lotesDoProdutoIds.Contains(s.LoteId))
                    .Sum(s => s.Qtd);
                estoqueConsolidado.Add(new Estoque
                {
                    ProdutoId = produto.IdProduto,
                    NomeProduto = produto.Nome,
                    QtdEntrada = totalEntrada,
                    QtdSaida = totalSaida,
                    SaldoAtual = totalEntrada - totalSaida
                });
            }
            return estoqueConsolidado.OrderBy(p => p.NomeProduto).ToList();
        }
    }
}