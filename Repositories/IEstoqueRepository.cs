using EmurbEstoque.Models;

namespace EmurbEstoque.Repositories
{
    public interface IEstoqueRepository
    {
        int GetQuantidadeByProdutoId(int produtoId);
        int UpdateQuantidade(int produtoId, int quantidadeDelta);
        void CreateOrUpdateEstoqueEntry(int produtoId, int quantidadeInicial = 0);
        List<Estoque> GetAll();
    }
}