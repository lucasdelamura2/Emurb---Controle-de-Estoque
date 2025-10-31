using EmurbEstoque.Models;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace EmurbEstoque.Repositories
{
    public class OrdemSaidaMemoryRepository : IOrdemSaidaRepository
    {
        private static readonly List<OrdemSaida> _ordensSaidaMem = new();
        private static int _nextOrdemId = 1;

        public OrdemSaida Create(OrdemSaida ordemSaida)
        {
            if (ordemSaida == null) throw new ArgumentNullException(nameof(ordemSaida));

            ordemSaida.IdOrdSai = _nextOrdemId++;
            ordemSaida.Status = "Aberta"; 
            _ordensSaidaMem.Add(ordemSaida);
            return ordemSaida; 
        }

        public List<OrdemSaida> GetAll()
        {
            return _ordensSaidaMem.OrderByDescending(o => o.DataSaida).ToList();
        }

        public OrdemSaida? GetById(int id)
        {
            return _ordensSaidaMem.FirstOrDefault(o => o.IdOrdSai == id);
        }
        
        // ADICIONE ESTE MÉTODO:
        public void Concluir(int id)
        {
            var ordem = _ordensSaidaMem.FirstOrDefault(o => o.IdOrdSai == id);
            if (ordem != null)
            {
                ordem.Status = "Concluída";
            }
        }
    }
}