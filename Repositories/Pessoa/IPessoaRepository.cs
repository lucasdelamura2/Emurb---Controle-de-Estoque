using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IPessoaRepository
    {
        Pessoa? GetById(int id);
        List<Pessoa> GetAll();
        void Update(Pessoa pessoa); 
        bool CpfCnpjExists(string cpfCnpj);
    }
}