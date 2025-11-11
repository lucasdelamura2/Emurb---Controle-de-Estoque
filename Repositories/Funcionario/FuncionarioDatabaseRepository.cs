using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;

namespace EmurbEstoque.Repositories
{
    public class FuncionarioDatabaseRepository : DbConnection, IFuncionarioRepository
    {
        public FuncionarioDatabaseRepository(string connStr) : base(connStr) { }

        public int Create(Funcionario model) 
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "cadFuncionario";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nome", model.Nome);
            cmd.Parameters.AddWithValue("@cpf_cnpj", model.CpfCnpj);
            cmd.Parameters.AddWithValue("@email", model.Email);
            cmd.Parameters.AddWithValue("@telefone", model.Telefone);
            cmd.Parameters.AddWithValue("@cargo", model.Cargo);
            cmd.Parameters.AddWithValue("@setor", model.Setor);

            var returnParameter = new SqlParameter();
            returnParameter.ParameterName = "@ReturnCode";
            returnParameter.SqlDbType = SqlDbType.Int;
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnParameter);
            cmd.ExecuteNonQuery();
            int statusCode = (int)returnParameter.Value;
            return statusCode;
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
        public int Update(Funcionario model)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "alterFuncionario";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idFuncionario", model.IdFuncionario);
            cmd.Parameters.AddWithValue("@nome", model.Nome);
            cmd.Parameters.AddWithValue("@cpf_cnpj", model.CpfCnpj);
            cmd.Parameters.AddWithValue("@email", model.Email);
            cmd.Parameters.AddWithValue("@telefone", model.Telefone);
            cmd.Parameters.AddWithValue("@cargo", model.Cargo);
            cmd.Parameters.AddWithValue("@setor", model.Setor);

            var returnParameter = new SqlParameter();
            returnParameter.SqlDbType = SqlDbType.Int;
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnParameter);

            try
            {
                cmd.ExecuteNonQuery();

                int statusCode = (int)returnParameter.Value;
                return statusCode;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return 2;
            }
        }
        public int Delete(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "delFuncionario"; 
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idFuncionario", id);

            var returnParameter = new SqlParameter();
            returnParameter.SqlDbType = SqlDbType.Int;
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnParameter);
            try
            {
                cmd.ExecuteNonQuery();
                int statusCode = (int)returnParameter.Value;
                return statusCode; 
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return 2; 
            }
        }
    }
}