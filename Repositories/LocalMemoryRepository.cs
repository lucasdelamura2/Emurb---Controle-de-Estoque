using EmurbEstoque.Models;
using System; 
using System.Collections.Generic; 
using System.Linq; 

namespace EmurbEstoque.Repositories
{
    public class LocalMemoryRepository : ILocalRepository
    {
        private static readonly List<Local> _locaisMem = new();
        private static int _nextId = 1;

        public void Create(Local local)
        {
            if (local == null) throw new ArgumentNullException(nameof(local));
            local.IdLocal = _nextId++;
            _locaisMem.Add(local);
        }

        public List<Local> GetAll()
        {
            return _locaisMem.OrderBy(l => l.Nome).ToList();
        }

        public Local? GetById(int id)
        {
            return _locaisMem.FirstOrDefault(l => l.IdLocal == id);
        }

        public void Update(Local dados)
        {
            var localExistente = GetById(dados.IdLocal);
            if (localExistente == null) return; 

            localExistente.Nome = dados.Nome;
            localExistente.Descricao = dados.Descricao;
        }

        public void Delete(int id)
        {
            var localParaDeletar = GetById(id);
            if (localParaDeletar != null)
            {
                _locaisMem.Remove(localParaDeletar);
            }
        }
    }
}