using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public class AutorizacaoDatabaseRepository : DbConnection, IAutorizacaoRepository
    {
        public AutorizacaoDatabaseRepository(string connStr) : base(connStr) { }

        public void Create(Autorizacao autorizacao)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO Autorizacao (autorizadoId, localId) VALUES (@autoId, @localId)";
            
            cmd.Parameters.AddWithValue("@autoId", autorizacao.AutorizadoId);
            cmd.Parameters.AddWithValue("@localId", autorizacao.LocalId);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM Autorizacao WHERE idAutoriza = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public bool Exists(int autorizadoId, int localId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT COUNT(1) FROM Autorizacao WHERE autorizadoId = @autoId AND localId = @localId";

            cmd.Parameters.AddWithValue("@autoId", autorizadoId);
            cmd.Parameters.AddWithValue("@localId", localId);

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            
            return count > 0;
        }

        public List<Autorizacao> GetAll()
        {
            var lista = new List<Autorizacao>(); 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Autorizacao";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Autorizacao
                    {
                        IdAutoriza = (int)reader["idAutoriza"],
                        AutorizadoId = (int)reader["autorizadoId"],
                        LocalId = (int)reader["localId"]
                    });
                }
            } 
            return lista;
        }

        public Autorizacao? GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Autorizacao WHERE idAutoriza = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Autorizacao
                    {
                        IdAutoriza = (int)reader["idAutoriza"],
                        AutorizadoId = (int)reader["autorizadoId"],
                        LocalId = (int)reader["localId"]
                    };
                }
            } 
            return null;
        }

        public void Update(Autorizacao autorizacao)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"UPDATE Autorizacao 
                                SET autorizadoId = @autoId, 
                                    localId = @localId 
                                WHERE idAutoriza = @id";
            
            cmd.Parameters.AddWithValue("@id", autorizacao.IdAutoriza);
            cmd.Parameters.AddWithValue("@autoId", autorizacao.AutorizadoId);
            cmd.Parameters.AddWithValue("@localId", autorizacao.LocalId);

            cmd.ExecuteNonQuery();
        }
    }
}