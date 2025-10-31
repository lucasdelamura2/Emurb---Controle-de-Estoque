using EmurbEstoque.Models;
using System; 
using System.Collections.Generic; 
using System.Linq; 

namespace EmurbEstoque.Repositories
{
    public class ItensOSMemoryRepository : IItensOSRepository
    {
        private static readonly List<ItensOS> _itensSaidaMem = new();
        private static int _nextItemId = 1;

        public void Create(ItensOS itemOS)
        {
            if (itemOS == null) throw new ArgumentNullException(nameof(itemOS));
            itemOS.IdItemOS = _nextItemId++;
            _itensSaidaMem.Add(itemOS);
        }

        public List<ItensOS> GetAll()
        {
            return _itensSaidaMem.ToList();
        }

        public ItensOS? GetById(int idItemOS)
        {
             return _itensSaidaMem.FirstOrDefault(i => i.IdItemOS == idItemOS);
        }

        public List<ItensOS> GetByOrdemSaidaId(int ordemSaidaId)
        {
            return _itensSaidaMem.Where(i => i.OrdSaiId == ordemSaidaId).ToList();
        }

        public void Delete(int idItemOS)
        {
             var itemParaDeletar = GetById(idItemOS);
            if (itemParaDeletar != null)
            {
                _itensSaidaMem.Remove(itemParaDeletar);
            }
        }
    }
}