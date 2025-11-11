using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IFornecedorRepository
    {
        int Create(Fornecedor fornecedor);
        List<Fornecedor> Read();
        Fornecedor? Read(int id);       
        int Update(Fornecedor fornecedor);
        int Delete(int id);
    }
}