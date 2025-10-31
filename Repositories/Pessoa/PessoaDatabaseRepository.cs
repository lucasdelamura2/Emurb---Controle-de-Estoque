using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmurbEstoque.Repositories
{
    public class PessoaDatabaseRepository : DbConnection, IPessoaRepository
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public PessoaDatabaseRepository(
            string connStr,
            IFuncionarioRepository funcionarioRepository,
            IFornecedorRepository fornecedorRepository
        ) : base(connStr)
        {
            _funcionarioRepository = funcionarioRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        public Pessoa? GetById(int id)
        {
            Pessoa? pessoa = _funcionarioRepository.Read(id);
            if (pessoa == null)
                pessoa = _fornecedorRepository.Read(id);

            return pessoa;
        }

        public List<Pessoa> GetAll()
        {
            var todas = new List<Pessoa>();
            todas.AddRange(_funcionarioRepository.Read());
            todas.AddRange(_fornecedorRepository.Read());
            return todas.OrderBy(p => p.Nome).ToList();
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
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"
                    UPDATE Pessoas SET
                        nome = @nome,
                        cpf_cnpj = @cpf_cnpj,
                        email = @email,
                        telefone = @telefone
                    WHERE idPessoa = @idPessoa;
                ";

                cmd.Parameters.AddWithValue("@idPessoa", pessoa.IdPessoa);
                cmd.Parameters.AddWithValue("@nome", pessoa.Nome);
                cmd.Parameters.AddWithValue("@cpf_cnpj", pessoa.CpfCnpj);
                cmd.Parameters.AddWithValue("@email", pessoa.Email);
                cmd.Parameters.AddWithValue("@telefone", pessoa.Telefone);

                cmd.ExecuteNonQuery();
            }
        }

        public bool CpfCnpjExists(string cpfCnpj)
        {
            if (string.IsNullOrWhiteSpace(cpfCnpj))
                return false;

            bool exists = false;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT COUNT(*) FROM Pessoas WHERE cpf_cnpj = @cpf_cnpj";
            cmd.Parameters.AddWithValue("@cpf_cnpj", cpfCnpj);

            int count = (int)cmd.ExecuteScalar();
            exists = count > 0;

            if (!exists)
            {
                var funcionarios = _funcionarioRepository.Read();
                var fornecedores = _fornecedorRepository.Read();

                exists = funcionarios.Any(f => f.CpfCnpj.Equals(cpfCnpj, StringComparison.OrdinalIgnoreCase))
                       || fornecedores.Any(f => f.CpfCnpj.Equals(cpfCnpj, StringComparison.OrdinalIgnoreCase));
            }

            return exists;
        }
    }
}
