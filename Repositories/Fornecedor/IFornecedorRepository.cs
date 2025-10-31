using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IFornecedorRepository
    {
        void Create(Fornecedor fornecedor);
        List<Fornecedor> Read();
        Fornecedor? Read(int id);       
        void Update(Fornecedor fornecedor);
        void Delete(int id);
    }
}