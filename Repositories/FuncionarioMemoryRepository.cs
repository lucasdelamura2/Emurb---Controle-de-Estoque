using EmurbEstoque.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmurbEstoque.Repositories
{
    public class FuncionarioMemoryRepository : IFuncionarioRepository
    {
        public static readonly List<Funcionario> _mem = new();
        private static int _nextId = 1;
        public void Create(Funcionario funcionario)
        {
            if (funcionario == null) throw new ArgumentNullException(nameof(funcionario));

            funcionario.IdPessoa = _nextId++; 
            _mem.Add(funcionario);
        }
        public void Delete(int id)
        {
            var f = Read(id);
            if (f != null) _mem.Remove(f);
        }
        public List<Funcionario> Read()
        {
            return _mem.OrderBy(f => f.Nome).ToList();
        }
        public Funcionario? Read(int id)
        {
            return _mem.FirstOrDefault(x => x.IdPessoa == id);
        }
        public void Update(Funcionario dados)
        {
            var f = Read(dados.IdPessoa); 
            if (f == null) return;
            f.Nome = dados.Nome;
            f.CpfCnpj = dados.CpfCnpj;
            f.Email = dados.Email;
            f.Telefone = dados.Telefone;
            f.Cargo = dados.Cargo;
            f.Setor = dados.Setor;
        }
    }
}