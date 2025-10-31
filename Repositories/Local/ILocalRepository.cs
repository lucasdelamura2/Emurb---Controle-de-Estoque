using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface ILocalRepository
    {
        void Create(Local local);
        List<Local> GetAll();
        Local? GetById(int id);
        void Update(Local local); 
        void Delete(int id);
    }
}