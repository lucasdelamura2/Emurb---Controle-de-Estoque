using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IAutorizacaoRepository
    {
        void Create(Autorizacao autorizacao);
        List<Autorizacao> GetAll();
        Autorizacao? GetById(int id); 
        void Delete(int id); 
        bool Exists(int autorizadoId, int localId);
        void Update(Autorizacao autorizacao);
    }
}