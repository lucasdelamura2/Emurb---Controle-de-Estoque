using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public class ItensOSDatabaseRepository : DbConnection, IItensOSRepository
    {
        public ItensOSDatabaseRepository(string connStr) : base(connStr) { }

        public void Create(ItensOS itemOS)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                INSERT INTO ItensOS (ordsaiId, loteId, qtd) 
                VALUES (@ordId, @loteId, @qtd)
            ";
            
            cmd.Parameters.AddWithValue("@ordId", itemOS.OrdSaiId); 
            cmd.Parameters.AddWithValue("@loteId", itemOS.LoteId);
            cmd.Parameters.AddWithValue("@qtd", itemOS.Qtd);

            cmd.ExecuteNonQuery();
        }

        public List<ItensOS> GetByOrdemSaidaId(int ordemSaidaId)
        {
            var lista = new List<ItensOS>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM ItensOS WHERE ordsaiId = @ordId";
            cmd.Parameters.AddWithValue("@ordId", ordemSaidaId);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new ItensOS
                    {
                        IdItemOS = (int)reader["idItemOS"],
                        OrdSaiId = (int)reader["ordsaiId"],
                        LoteId = (int)reader["loteId"],
                        Qtd = (int)reader["qtd"]
                    });
                }
            }
            return lista;
        }

        public List<ItensOS> GetAll()
        {
            var lista = new List<ItensOS>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM ItensOS";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new ItensOS
                    {
                        IdItemOS = (int)reader["idItemOS"],
                        OrdSaiId = (int)reader["ordsaiId"],
                        LoteId = (int)reader["loteId"],
                        Qtd = (int)reader["qtd"]
                    });
                }
            }
            return lista;
        }

        public ItensOS? GetById(int idItemOS)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM ItensOS WHERE idItemOS = @id";
            cmd.Parameters.AddWithValue("@id", idItemOS);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new ItensOS
                    {
                        IdItemOS = (int)reader["idItemOS"],
                        OrdSaiId = (int)reader["ordsaiId"],
                        LoteId = (int)reader["loteId"],
                        Qtd = (int)reader["qtd"]
                    };
                }
            }
            return null;
        }

        public void Delete(int idItemOS)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM ItensOS WHERE idItemOS = @id";
            cmd.Parameters.AddWithValue("@id", idItemOS);
            cmd.ExecuteNonQuery();
        }
    }
}