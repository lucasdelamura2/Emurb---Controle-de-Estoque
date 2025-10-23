using EmurbEstoque.Models;
using System; 
using System.Collections.Generic; 
using System.Linq; 

namespace EmurbEstoque.Repositories
{
    public class AutorizadoMemoryRepository : IAutorizadoRepository
    {
        private static readonly List<Autorizado> _autorizadosMem = new();
        private static int _nextId = 1;

        public void Create(Autorizado autorizado)
        {
            if (autorizado == null) throw new ArgumentNullException(nameof(autorizado));
            autorizado.IdAutorizado = _nextId++;
            _autorizadosMem.Add(autorizado);
        }

        public List<Autorizado> GetAll()
        {
            return _autorizadosMem.OrderBy(a => a.Funcao).ToList();
        }

        public Autorizado? GetById(int id)
        {
            return _autorizadosMem.FirstOrDefault(a => a.IdAutorizado == id);
        }

        public void Update(Autorizado dados)
        {
            var autorizadoExistente = GetById(dados.IdAutorizado);
            if (autorizadoExistente == null) return;

            autorizadoExistente.Funcao = dados.Funcao;
        }

        public void Delete(int id)
        {
            var autorizadoParaDeletar = GetById(id);
            if (autorizadoParaDeletar != null)
            {
                _autorizadosMem.Remove(autorizadoParaDeletar);
            }
        }
    }
}