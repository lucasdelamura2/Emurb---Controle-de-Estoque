using EmurbEstoque.Models;
using System; 
using System.Collections.Generic; 
using System.Linq; 

namespace EmurbEstoque.Repositories
{
    public class ProdutoMemoryRepository : IProdutoRepository
    {
        private static readonly List<Produto> _produtosMem = new();
        private static int _nextId = 1;
        public void Create(Produto produto)
        {
            if (produto == null) throw new ArgumentNullException(nameof(produto));
            produto.IdProduto = _nextId++;
            _produtosMem.Add(produto);
        }
        public List<Produto> GetAll()
        {
            return _produtosMem.OrderBy(p => p.Nome).ToList();
        }
        public Produto? GetById(int id)
        {
            return _produtosMem.FirstOrDefault(p => p.IdProduto == id);
        }
        public void Update(Produto dados)
        {
            if (dados == null) throw new ArgumentNullException(nameof(dados));

            var produtoExistente = GetById(dados.IdProduto);
            if (produtoExistente == null)
            {
                return;
            }
            produtoExistente.Nome = dados.Nome;
            produtoExistente.Descricao = dados.Descricao;
        }
        public void Delete(int id)
        {
            var produtoParaDeletar = GetById(id);
            if (produtoParaDeletar != null)
            {
                _produtosMem.Remove(produtoParaDeletar);
            }
        }
    }
}