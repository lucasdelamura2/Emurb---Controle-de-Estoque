using EmurbEstoque.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmurbEstoque.Repositories
{
    public class PessoaMemoryRepository : IPessoaRepository
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        public PessoaMemoryRepository(IFuncionarioRepository funcionarioRepository, IFornecedorRepository fornecedorRepository)
        {
             _funcionarioRepository = funcionarioRepository;
             _fornecedorRepository = fornecedorRepository;
        }
        public Pessoa? GetById(int id)
        {
            Pessoa? pessoa = _funcionarioRepository.Read(id);
            if (pessoa == null)
            {
                pessoa = _fornecedorRepository.Read(id);
            }
            return pessoa;
        }

        public List<Pessoa> GetAll()
        {
            var todasPessoas = new List<Pessoa>();
            todasPessoas.AddRange(_funcionarioRepository.Read());
            todasPessoas.AddRange(_fornecedorRepository.Read());
            return todasPessoas.OrderBy(p => p.Nome).ToList();
        }

        public void Update(Pessoa pessoa)
        {
            if (pessoa is Funcionario funcionario)
            {
                _funcionarioRepository.Update(funcionario);
            }
            else if (pessoa is Fornecedor fornecedor)
            {
                _fornecedorRepository.Update(fornecedor);
            }
        }

         public bool CpfCnpjExists(string cpfCnpj)
        {
             if (string.IsNullOrWhiteSpace(cpfCnpj)) return false;
             bool existsInFuncionarios = FuncionarioMemoryRepository._mem.Any(f => f.CpfCnpj.Equals(cpfCnpj, StringComparison.OrdinalIgnoreCase));
             bool existsInFornecedores = FornecedorMemoryRepository._mem.Any(f => f.CpfCnpj.Equals(cpfCnpj, StringComparison.OrdinalIgnoreCase));
             return existsInFuncionarios || existsInFornecedores;
        }
    }
}