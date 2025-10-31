using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IFuncionarioRepository
    {
        void Create(Funcionario funcionario);
        List<Funcionario> Read(); 
        Funcionario? Read(int id);        
        void Update(Funcionario funcionario);
        void Delete(int id);
    }
}