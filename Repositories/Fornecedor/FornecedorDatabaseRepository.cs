using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;

namespace EmurbEstoque.Repositories
{
    public class FornecedorDatabaseRepository : DbConnection, IFornecedorRepository
    {
        public FornecedorDatabaseRepository(string connStr) : base(connStr) { }

        public int Create(Fornecedor fornecedor)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "cadFornecedor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nome", fornecedor.Nome);
            cmd.Parameters.AddWithValue("@cpf_cnpj", fornecedor.CpfCnpj);
            cmd.Parameters.AddWithValue("@email", fornecedor.Email);
            cmd.Parameters.AddWithValue("@telefone", fornecedor.Telefone);
            cmd.Parameters.AddWithValue("@inscricao_estadual", fornecedor.InscricaoEstadual);

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

        public List<Fornecedor> Read()
        {
            var lista = new List<Fornecedor>(); 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            
            cmd.CommandText = @"
                SELECT * FROM Pessoas p
                JOIN Fornecedores f ON p.idPessoa = f.idFornecedor
                ORDER BY p.nome
            ";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    lista.Add(new Fornecedor
                    {
                        IdFornecedor = (int)reader["idPessoa"],
                        Nome = (string)reader["nome"],
                        CpfCnpj = (string)reader["cpf_cnpj"],
                        Email = (string)reader["email"],
                        Telefone = (string)reader["telefone"],
                        InscricaoEstadual = (string)reader["inscricao_estadual"]
                    });
                }
            } 
            return lista;
        }

        public Fornecedor? Read(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                SELECT * FROM Pessoas p
                JOIN Fornecedores f ON p.idPessoa = f.idFornecedor
                WHERE p.idPessoa = @id
            ";
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if(reader.Read())
                {
                     return new Fornecedor
                    {
                        IdFornecedor = (int)reader["idPessoa"],
                        Nome = (string)reader["nome"],
                        CpfCnpj = (string)reader["cpf_cnpj"],
                        Email = (string)reader["email"],
                        Telefone = (string)reader["telefone"],
                        InscricaoEstadual = (string)reader["inscricao_estadual"]
                    };
                }
            } 
            return null;
        }

        public int Update(Fornecedor fornecedor)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "alterFornecedor"; 
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idFornecedor", fornecedor.IdFornecedor);
            cmd.Parameters.AddWithValue("@nome", fornecedor.Nome);
            cmd.Parameters.AddWithValue("@cpf_cnpj", fornecedor.CpfCnpj);
            cmd.Parameters.AddWithValue("@email", fornecedor.Email);
            cmd.Parameters.AddWithValue("@telefone", fornecedor.Telefone);
            cmd.Parameters.AddWithValue("@inscricao_estadual", fornecedor.InscricaoEstadual);

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 2; 
            }
        }

        public int Delete(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "delFornecedor"; 
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idFornecedor", id);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 2;
            }
        }
    }    
}