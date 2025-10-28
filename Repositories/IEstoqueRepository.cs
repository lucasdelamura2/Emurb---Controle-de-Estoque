using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IEstoqueRepository
    {
        List<Estoque> GetEstoqueConsolidado();
    }
}