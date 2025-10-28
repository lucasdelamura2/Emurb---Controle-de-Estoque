using EmurbEstoque.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmurbEstoque.Repositories
{
    public class EstoqueMemoryRepository : IEstoqueRepository
    {
        private static readonly Dictionary<int, Estoque> _estoqueMem = new Dictionary<int, Estoque>();
        private static int _nextEstoqueId = 1;

        public int GetQuantidadeByProdutoId(int produtoId)
        {
            return _estoqueMem.TryGetValue(produtoId, out Estoque? estoque) ? estoque.QuantidadeAtual : 0;
        }

        public int UpdateQuantidade(int produtoId, int quantidadeDelta)
        {
            if (_estoqueMem.TryGetValue(produtoId, out Estoque? estoque))
            {
                estoque.QuantidadeAtual += quantidadeDelta;
                if (estoque.QuantidadeAtual < 0)
                {
                    estoque.QuantidadeAtual = 0;
                    System.Diagnostics.Debug.WriteLine($"Aviso: Tentativa de deixar estoque negativo para ProdutoId {produtoId}. Ajustado para 0.");
                }
                return estoque.QuantidadeAtual;
            }
            else
            {
                if (quantidadeDelta > 0)
                {
                     CreateOrUpdateEstoqueEntry(produtoId, quantidadeDelta);
                     return quantidadeDelta;
                }
                 return 0;
            }
        }

        public void CreateOrUpdateEstoqueEntry(int produtoId, int quantidadeInicial = 0)
        {
             if (_estoqueMem.TryGetValue(produtoId, out Estoque? estoque))
             {
                 estoque.QuantidadeAtual = quantidadeInicial >= 0 ? quantidadeInicial : 0;
             }
             else
             {
                 var novoEstoque = new Estoque
                 {
                     IdEstoque = _nextEstoqueId++,
                     ProdutoId = produtoId,
                     QuantidadeAtual = quantidadeInicial >= 0 ? quantidadeInicial : 0
                 };
                 _estoqueMem.Add(produtoId, novoEstoque);
             }
        }

        public List<Estoque> GetAll()
        {
            return _estoqueMem.Values.OrderBy(e => e.ProdutoId).ToList();
        }
    }
}