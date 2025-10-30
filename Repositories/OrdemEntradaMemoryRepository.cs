using EmurbEstoque.Models;

namespace EmurbEstoque.Repositories
{
    public class OrdemEntradaMemoryRepository : IOrdemEntradaRepository
    {
        private static readonly List<OrdemEntrada> _ordensMem = new();
        private static int _nextOrdemId = 1;

        public OrdemEntrada Create(OrdemEntrada ordem)
        {
            if (ordem == null) throw new ArgumentNullException(nameof(ordem));
            
            ordem.IdOrdEnt = _nextOrdemId++;
            ordem.Status = "Aberta"; 
            
            _ordensMem.Add(ordem);
            return ordem;
        }

        public List<OrdemEntrada> GetAll()
        {
            return _ordensMem.OrderByDescending(o => o.DataEnt).ToList();
        }

        public OrdemEntrada? GetById(int id)
        {
            return _ordensMem.FirstOrDefault(o => o.IdOrdEnt == id);
        }
        public void Concluir(int id)
        {
            var ordem = _ordensMem.FirstOrDefault(o => o.IdOrdEnt == id);
            if (ordem != null)
            {
                ordem.Status = "Concluída";
            }
        }
    }
}