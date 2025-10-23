using EmurbEstoque.Models;

namespace EmurbEstoque.Repositories
{
    public interface IOrdemEntradaRepository
    {
        OrdemEntrada Create(OrdemEntrada ordem);
        List<OrdemEntrada> GetAll();
        OrdemEntrada? GetById(int id);
    }
}
