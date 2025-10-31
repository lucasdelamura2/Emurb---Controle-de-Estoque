using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public class OrdemSaidaDatabaseRepository : DbConnection, IOrdemSaidaRepository
    {
        public OrdemSaidaDatabaseRepository(string connStr) : base(connStr) { }

        public OrdemSaida Create(OrdemSaida ordemSaida)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                INSERT INTO OrdemSaida (funcId, autorizaId, dataSaida, status) 
                VALUES (@funcId, @autoId, @data, @status);
                SELECT SCOPE_IDENTITY();
            ";

            cmd.Parameters.AddWithValue("@funcId", ordemSaida.IdFuncionario);
            cmd.Parameters.AddWithValue("@autoId", ordemSaida.AutorizaId);
            cmd.Parameters.AddWithValue("@data", ordemSaida.DataSaida);
            cmd.Parameters.AddWithValue("@status", ordemSaida.Status); 

            var newId = Convert.ToInt32(cmd.ExecuteScalar());
            ordemSaida.IdOrdSai = newId;
            return ordemSaida;
        }

        public List<OrdemSaida> GetAll()
        {
            var lista = new List<OrdemSaida>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM OrdemSaida ORDER BY dataSaida DESC";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new OrdemSaida
                    {
                        IdOrdSai = (int)reader["idOrdSai"],
                        IdFuncionario = (int)reader["funcId"],
                        AutorizaId = (int)reader["autorizaId"],
                        DataSaida = (DateTime)reader["dataSaida"],
                        Status = (string)reader["status"] 
                    });
                }
            }
            return lista;
        }

        public OrdemSaida? GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM OrdemSaida WHERE idOrdSai = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new OrdemSaida
                    {
                        IdOrdSai = (int)reader["idOrdSai"],
                        IdFuncionario = (int)reader["funcId"],
                        AutorizaId = (int)reader["autorizaId"],
                        DataSaida = (DateTime)reader["dataSaida"],
                        Status = (string)reader["status"]
                    };
                }
            }
            return null;
        }

        public void Concluir(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE OrdemSaida SET status = 'Concluída' WHERE idOrdSai = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}