using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public class LocalDatabaseRepository : DbConnection, ILocalRepository
    {
        public LocalDatabaseRepository(string connStr) : base(connStr) { }

        public void Create(Local local)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                INSERT INTO Locais (nome, descricao)
                VALUES (@nome, @descricao);
            ";

            cmd.Parameters.AddWithValue("@nome", local.Nome);
            cmd.Parameters.AddWithValue("@descricao", (object?)local.Descricao ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public List<Local> GetAll()
        {
            var lista = new List<Local>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Locais ORDER BY nome";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Local
                {
                    IdLocal = (int)reader["idLocal"],
                    Nome = (string)reader["nome"],
                    Descricao = reader["descricao"] == DBNull.Value ? null : (string)reader["descricao"]
                });
            }
            reader.Close();
            return lista;
        }

        public Local? GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Locais WHERE idLocal = @id";
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var local = new Local
                {
                    IdLocal = (int)reader["idLocal"],
                    Nome = (string)reader["nome"],
                    Descricao = reader["descricao"] == DBNull.Value ? null : (string)reader["descricao"]
                };
                reader.Close();
                return local;
            }
            reader.Close();
            return null;
        }

        public void Update(Local local)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                UPDATE Locais SET
                    nome = @nome,
                    descricao = @descricao
                WHERE idLocal = @id;
            ";

            cmd.Parameters.AddWithValue("@id", local.IdLocal);
            cmd.Parameters.AddWithValue("@nome", local.Nome);
            cmd.Parameters.AddWithValue("@descricao", (object?)local.Descricao ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM Locais WHERE idLocal = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
