using EmurbEstoque.Models;
using System.Collections.Generic; 

namespace EmurbEstoque.Repositories
{
    public interface IOrdemSaidaRepository
    {
        OrdemSaida Create(OrdemSaida ordemSaida);
        List<OrdemSaida> GetAll();
        OrdemSaida? GetById(int id);
        void Concluir(int id);
    }
}