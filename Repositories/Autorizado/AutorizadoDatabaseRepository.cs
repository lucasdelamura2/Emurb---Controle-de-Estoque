using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public class AutorizadoDatabaseRepository : DbConnection, IAutorizadoRepository
    {
        public AutorizadoDatabaseRepository(string connStr) : base(connStr) { }

        public void Create(Autorizado autorizado)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                INSERT INTO Autorizados (funcao)
                VALUES (@funcao);
            ";

            cmd.Parameters.AddWithValue("@funcao", autorizado.Funcao);
            cmd.ExecuteNonQuery();
        }

        public List<Autorizado> GetAll()
        {
            var lista = new List<Autorizado>(); 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Autorizados ORDER BY funcao;";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Autorizado
                    {
                        IdAutorizado = (int)reader["idAutorizado"],
                        Funcao = (string)reader["funcao"]
                    });
                }
            } 
            return lista;
        }

        public Autorizado? GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Autorizados WHERE idAutorizado = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var autorizado = new Autorizado
                    {
                        IdAutorizado = (int)reader["idAutorizado"],
                        Funcao = (string)reader["funcao"]
                    };
                    return autorizado;
                }
            } 
            return null;
        }

        public void Update(Autorizado autorizado)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                UPDATE Autorizados
                SET funcao = @funcao
                WHERE idAutorizado = @id;
            ";

            cmd.Parameters.AddWithValue("@id", autorizado.IdAutorizado);
            cmd.Parameters.AddWithValue("@funcao", autorizado.Funcao);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM Autorizados WHERE idAutorizado = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}