using EmurbEstoque.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmurbEstoque.Repositories
{
    public class FornecedorMemoryRepository : IFornecedorRepository
    {
        public static readonly List<Fornecedor> _mem = new();
        private static int _nextId = 1;
        public void Create(Fornecedor fornecedor)
        {
            if (fornecedor == null) throw new ArgumentNullException(nameof(fornecedor));
            fornecedor.IdPessoa = _nextId++;
            _mem.Add(fornecedor);
        }
        public void Delete(int id)
        {
            var f = Read(id);
            if (f != null) _mem.Remove(f);
        }
        public List<Fornecedor> Read()
        {
            return _mem.OrderBy(f => f.Nome).ToList();
        }
        public Fornecedor? Read(int id)
        {
            return _mem.FirstOrDefault(x => x.IdPessoa == id);
        }
        public void Update(Fornecedor dados)
        {
            var f = Read(dados.IdPessoa);
            if (f == null) return;

            f.Nome = dados.Nome;
            f.CpfCnpj = dados.CpfCnpj;
            f.Email = dados.Email;
            f.Telefone = dados.Telefone;
            f.InscricaoEstadual = dados.InscricaoEstadual;
        }
    }
}