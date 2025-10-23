using EmurbEstoque.Models;

namespace EmurbEstoque.Repositories
{
    public class LoteMemoryRepository : ILoteRepository
    {
        private static readonly List<Lote> _lotesMem = new();
        private static int _nextLoteId = 1;

        public void Create(Lote lote)
        {
            if (lote == null) throw new ArgumentNullException(nameof(lote));
            lote.IdLote = _nextLoteId++;
            _lotesMem.Add(lote);
        }

        public List<Lote> GetAll()
        {
            return _lotesMem.ToList(); 
        }

        public Lote? GetById(int id)
        {
            return _lotesMem.FirstOrDefault(l => l.IdLote == id);
        }

        public List<Lote> GetByOrdemEntradaId(int ordemEntradaId)
        {
            return _lotesMem.Where(l => l.OrdEntId == ordemEntradaId).ToList();
        }
    }
}
