using EmurbEstoque.Models;

namespace EmurbEstoque.Repositories
{
    public interface IProdutoRepository
    {
        void Create(Produto produto);
        List<Produto> GetAll();
        Produto? GetById(int id);
        void Update(Produto produto); 
        void Delete(int id);         
    }
}