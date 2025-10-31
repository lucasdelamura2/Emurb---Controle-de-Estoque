using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic; 
using System; 

namespace EmurbEstoque.Repositories
{
    public class OrdemEntradaDatabaseRepository : DbConnection, IOrdemEntradaRepository
    {
        public OrdemEntradaDatabaseRepository(string connStr) : base(connStr) { }

        public OrdemEntrada Create(OrdemEntrada ordem)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                INSERT INTO OrdemEntrada (fornId, dataEnt, status) 
                VALUES (@fornId, @dataEnt, @status);
                SELECT SCOPE_IDENTITY(); 
            ";
            
            cmd.Parameters.AddWithValue("@fornId", ordem.IdFornecedor);
            cmd.Parameters.AddWithValue("@dataEnt", ordem.DataEnt);
            cmd.Parameters.AddWithValue("@status", ordem.Status); 

            var newId = Convert.ToInt32(cmd.ExecuteScalar());
            ordem.IdOrdEnt = newId;
            return ordem;
        }

        public List<OrdemEntrada> GetAll()
        {
            var lista = new List<OrdemEntrada>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM OrdemEntrada ORDER BY dataEnt DESC";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new OrdemEntrada
                    {
                        IdOrdEnt = (int)reader["idOrdEnt"],
                        IdFornecedor = (int)reader["fornId"],
                        DataEnt = (DateTime)reader["dataEnt"],
                        Status = (string)reader["status"] 
                    });
                }
            } 
            return lista;
        }

        public OrdemEntrada? GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM OrdemEntrada WHERE idOrdEnt = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new OrdemEntrada
                    {
                        IdOrdEnt = (int)reader["idOrdEnt"],
                        IdFornecedor = (int)reader["fornId"],
                        DataEnt = (DateTime)reader["dataEnt"],
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
            cmd.CommandText = "UPDATE OrdemEntrada SET status = 'Concluída' WHERE idOrdEnt = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}