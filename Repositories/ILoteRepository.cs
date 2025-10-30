using EmurbEstoque.Models;

namespace EmurbEstoque.Repositories
{
    public interface ILoteRepository
    {
        void Create(Lote lote);
        List<Lote> GetByOrdemEntradaId(int ordemEntradaId);
        List<Lote> GetAll();
        Lote? GetById(int id);
        void Delete(int id);
        void Update(Lote lote);
    }
}
