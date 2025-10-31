using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic; 

namespace EmurbEstoque.Repositories
{
    public class FuncionarioDatabaseRepository : DbConnection, IFuncionarioRepository
    {
        public FuncionarioDatabaseRepository(string connStr) : base(connStr) { }

        public void Create(Funcionario funcionario)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                INSERT INTO Pessoas (nome, cpf_cnpj, email, telefone) 
                VALUES (@nome, @cpf, @email, @tel);

                DECLARE @newId INT = SCOPE_IDENTITY();
                
                INSERT INTO Funcionarios (idFuncionario, cargo, setor)
                VALUES (@newId, @cargo, @setor);
            ";
            cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
            cmd.Parameters.AddWithValue("@cpf", funcionario.CpfCnpj); 
            cmd.Parameters.AddWithValue("@email", funcionario.Email);
            cmd.Parameters.AddWithValue("@tel", funcionario.Telefone);
            cmd.Parameters.AddWithValue("@cargo", funcionario.Cargo);
            cmd.Parameters.AddWithValue("@setor", funcionario.Setor);

            cmd.ExecuteNonQuery();
        }
        public List<Funcionario> Read()
        {
            var lista = new List<Funcionario>(); 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                SELECT * FROM Pessoas p
                JOIN Funcionarios f ON p.idPessoa = f.idFuncionario
                ORDER BY p.nome
            ";
            
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    lista.Add(new Funcionario
                    {
                        IdFuncionario = (int)reader["idPessoa"],
                        Nome = (string)reader["nome"],
                        CpfCnpj = (string)reader["cpf_cnpj"],
                        Email = (string)reader["email"],
                        Telefone = (string)reader["telefone"],
                        Cargo = (string)reader["cargo"],
                        Setor = (string)reader["setor"]
                    });
                }
            }
            return lista;
        }

        public Funcionario? Read(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                SELECT * FROM Pessoas p
                JOIN Funcionarios f ON p.idPessoa = f.idFuncionario
                WHERE p.idPessoa = @id
            ";
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if(reader.Read())
                {
                     return new Funcionario
                    {
                        IdFuncionario = (int)reader["idPessoa"],
                        Nome = (string)reader["nome"],
                        CpfCnpj = (string)reader["cpf_cnpj"],
                        Email = (string)reader["email"],
                        Telefone = (string)reader["telefone"],
                        Cargo = (string)reader["cargo"],
                        Setor = (string)reader["setor"]
                    };
                }
            }
            return null;
        }
        public void Update(Funcionario funcionario)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                UPDATE Pessoas SET
                    nome = @nome,
                    cpf_cnpj = @cpf,
                    email = @email,
                    telefone = @tel
                WHERE idPessoa = @id;

                UPDATE Funcionarios SET
                    cargo = @cargo,
                    setor = @setor
                WHERE idFuncionario = @id;
            ";

            cmd.Parameters.AddWithValue("@id", funcionario.IdFuncionario);
            cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
            cmd.Parameters.AddWithValue("@cpf", funcionario.CpfCnpj);
            cmd.Parameters.AddWithValue("@email", funcionario.Email);
            cmd.Parameters.AddWithValue("@tel", funcionario.Telefone);
            cmd.Parameters.AddWithValue("@cargo", funcionario.Cargo);
            cmd.Parameters.AddWithValue("@setor", funcionario.Setor);

            cmd.ExecuteNonQuery();
        }
        public void Delete(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                DELETE FROM Funcionarios WHERE idFuncionario = @id;
                DELETE FROM Pessoas WHERE idPessoa = @id;
            ";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}