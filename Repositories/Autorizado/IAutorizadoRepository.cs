using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IAutorizadoRepository
    {
        void Create(Autorizado autorizado);
        List<Autorizado> GetAll();
        Autorizado? GetById(int id);
        void Update(Autorizado autorizado); 
        void Delete(int id);               
    }
}