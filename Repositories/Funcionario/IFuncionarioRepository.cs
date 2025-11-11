using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IFuncionarioRepository
    {
        int Create(Funcionario funcionario);
        List<Funcionario> Read(); 
        Funcionario? Read(int id);        
        int Update(Funcionario funcionario);
        int Delete(int id);
    }
}