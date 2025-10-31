using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public class ItensOSDatabaseRepository : DbConnection, IItensOSRepository 
    {
        public ItensOSDatabaseRepository(string connStr) : base(connStr) { }

        public void Create(ItensOS itemOS)
        {
            if (itemOS == null)
                 throw new ArgumentNullException(nameof(itemOS));

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                INSERT INTO ItensOS (ordsaiId, loteId, qtd)
                VALUES (@ordsaiId, @loteId, @qtd);
            ";
            cmd.Parameters.AddWithValue("@ordsaiId", itemOS.OrdSaiId);
            cmd.Parameters.AddWithValue("@loteId", itemOS.LoteId);
            cmd.Parameters.AddWithValue("@qtd", itemOS.Qtd);

            cmd.ExecuteNonQuery();
        }

        public List<ItensOS> GetAll()
        {
            var lista = new List<ItensOS>(); 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                SELECT idItemOS, ordsaiId, loteId, qtd
                FROM ItensOS
                ORDER BY idItemOS;
            ";
            
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
            cmd.CommandText = @"
                SELECT idItemOS, ordsaiId, loteId, qtd
                FROM ItensOS
                WHERE idItemOS = @idItemOS;
            ";
            cmd.Parameters.AddWithValue("@idItemOS", idItemOS);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var item = new ItensOS
                    {
                        IdItemOS = (int)reader["idItemOS"],
                        OrdSaiId = (int)reader["ordsaiId"],
                        LoteId = (int)reader["loteId"],
                        Qtd = (int)reader["qtd"]
                    };
                    return item;
                }
            } 
            return null;
        }

        public List<ItensOS> GetByOrdemSaidaId(int ordemSaidaId)
        {
            var lista = new List<ItensOS>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                SELECT idItemOS, ordsaiId, loteId, qtd
                FROM ItensOS
                WHERE ordsaiId = @ordsaiId
                ORDER BY idItemOS;
            ";
            cmd.Parameters.AddWithValue("@ordsaiId", ordemSaidaId);

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

        public void Delete(int idItemOS)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM ItensOS WHERE idItemOS = @idItemOS;";
            cmd.Parameters.AddWithValue("@idItemOS", idItemOS);
            cmd.ExecuteNonQuery();
        }
    }
}