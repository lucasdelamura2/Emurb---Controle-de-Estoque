using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IItensOSRepository
    {
        void Create(ItensOS itemOS);
        List<ItensOS> GetByOrdemSaidaId(int ordemSaidaId);
        List<ItensOS> GetAll(); 
        ItensOS? GetById(int idItemOS);
        void Delete(int idItemOS);  
    }
}