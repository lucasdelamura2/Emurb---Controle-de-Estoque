using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public class PessoaDatabaseRepository : DbConnection, IPessoaRepository
    {
        public PessoaDatabaseRepository(string connStr) : base(connStr) { }

        public List<Pessoa> GetAll()
        {
            var todas = new List<Pessoa>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                SELECT *, 
                       CASE WHEN f.idFuncionario IS NOT NULL THEN 'Funcionario'
                            WHEN fr.idFornecedor IS NOT NULL THEN 'Fornecedor'
                            ELSE 'Pessoa' END AS Tipo
                FROM Pessoas p
                LEFT JOIN Funcionarios f ON p.idPessoa = f.idFuncionario
                LEFT JOIN Fornecedores fr ON p.idPessoa = fr.idFornecedor
                ORDER BY p.nome
            ";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tipo = (string)reader["Tipo"];
                    
                    if (tipo == "Funcionario")
                    {
                        todas.Add(new Funcionario
                        {
                            IdPessoa = (int)reader["idPessoa"],
                            Nome = (string)reader["nome"],
                            CpfCnpj = (string)reader["cpf_cnpj"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            Cargo = (string)reader["cargo"],
                            Setor = (string)reader["setor"]
                        });
                    }
                    else if (tipo == "Fornecedor")
                    {
                         todas.Add(new Fornecedor
                        {
                            IdPessoa = (int)reader["idPessoa"],
                            Nome = (string)reader["nome"],
                            CpfCnpj = (string)reader["cpf_cnpj"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            InscricaoEstadual = (string)reader["inscricao_estadual"]
                        });
                    }
                }
            }
            return todas;
        }

        public Pessoa? GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                SELECT *, 
                       CASE WHEN f.idFuncionario IS NOT NULL THEN 'Funcionario'
                            WHEN fr.idFornecedor IS NOT NULL THEN 'Fornecedor'
                            ELSE 'Pessoa' END AS Tipo
                FROM Pessoas p
                LEFT JOIN Funcionarios f ON p.idPessoa = f.idFuncionario
                LEFT JOIN Fornecedores fr ON p.idPessoa = fr.idFornecedor
                WHERE p.idPessoa = @id
            ";
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string tipo = (string)reader["Tipo"];
                    
                    if (tipo == "Funcionario")
                    {
                        return new Funcionario
                        {
                            IdPessoa = (int)reader["idPessoa"],
                            Nome = (string)reader["nome"],
                            CpfCnpj = (string)reader["cpf_cnpj"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            Cargo = (string)reader["cargo"],
                            Setor = (string)reader["setor"]
                        };
                    }
                    else if (tipo == "Fornecedor")
                    {
                         return new Fornecedor
                        {
                            IdPessoa = (int)reader["idPessoa"],
                            Nome = (string)reader["nome"],
                            CpfCnpj = (string)reader["cpf_cnpj"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            InscricaoEstadual = (string)reader["inscricao_estadual"]
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Pessoa pessoa)
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

        public bool CpfCnpjExists(string cpfCnpj)
        {
            if (string.IsNullOrWhiteSpace(cpfCnpj))
                return false;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT COUNT(1) FROM Pessoas WHERE cpf_cnpj = @cpf_cnpj";
            cmd.Parameters.AddWithValue("@cpf_cnpj", cpfCnpj);

            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }
}