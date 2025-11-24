using EmurbEstoque.Models;
using System; 
using System.Collections.Generic; 
using System.Linq; 

namespace EmurbEstoque.Repositories
{
    public class AutorizacaoMemoryRepository : IAutorizacaoRepository
    {
        private static readonly List<Autorizacao> _autorizacoesMem = new();
        private static int _nextId = 1;

        public void Create(Autorizacao autorizacao)
        {
            if (autorizacao == null) throw new ArgumentNullException(nameof(autorizacao));
            if (Exists(autorizacao.AutorizadoId, autorizacao.LocalId))
            {
                 Console.WriteLine($"Aviso: Tentativa de adicionar autorização duplicada para AutorizadoId={autorizacao.AutorizadoId}, LocalId={autorizacao.LocalId}");
                 return; 
            }

            autorizacao.IdAutoriza = _nextId++;
            _autorizacoesMem.Add(autorizacao);
        }

        public List<Autorizacao> GetAll()
        {
            return _autorizacoesMem.ToList(); 
        }

        public Autorizacao? GetById(int id)
        {
            return _autorizacoesMem.FirstOrDefault(a => a.IdAutoriza == id);
        }

        public void Delete(int id)
        {
            var autorizacaoParaDeletar = GetById(id);
            if (autorizacaoParaDeletar != null)
            {
                _autorizacoesMem.Remove(autorizacaoParaDeletar);
            }
        }

        public bool Exists(int autorizadoId, int localId)
        {
            return _autorizacoesMem.Any(a => a.AutorizadoId == autorizadoId && a.LocalId == localId);
        }
        public void Update(Autorizacao autorizacao)
        {
            if (autorizacao == null) throw new ArgumentNullException(nameof(autorizacao));

            var autorizacaoExistente = GetById(autorizacao.IdAutoriza);
            if (autorizacaoExistente == null)
            {
                throw new InvalidOperationException("Autorização não encontrada para atualização.");
            }

            if (Exists(autorizacao.AutorizadoId, autorizacao.LocalId) &&
                (autorizacaoExistente.AutorizadoId != autorizacao.AutorizadoId ||
                 autorizacaoExistente.LocalId != autorizacao.LocalId))
            {
                throw new InvalidOperationException("Esta combinação de Função e Local já existe.");
            }
            autorizacaoExistente.AutorizadoId = autorizacao.AutorizadoId;
            autorizacaoExistente.LocalId = autorizacao.LocalId;
        }
    }
}