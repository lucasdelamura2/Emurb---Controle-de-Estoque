using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;

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
                INSERT INTO OrdemEntrada (fornId, dataEnt) 
                VALUES (@fornId, @dataEnt);
                SELECT SCOPE_IDENTITY(); 
            ";
            
            cmd.Parameters.AddWithValue("@fornId", ordem.IdFornecedor);
            cmd.Parameters.AddWithValue("@dataEnt", ordem.DataEnt);
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

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new OrdemEntrada
                {
                    IdOrdEnt = (int)reader["idOrdEnt"],
                    IdFornecedor = (int)reader["fornId"],
                    DataEnt = (DateTime)reader["dataEnt"]
                });
            }
            return lista;
        }

        public OrdemEntrada? GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM OrdemEntrada WHERE idOrdEnt = @id";
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new OrdemEntrada
                {
                    IdOrdEnt = (int)reader["idOrdEnt"],
                    IdFornecedor = (int)reader["fornId"],
                    DataEnt = (DateTime)reader["dataEnt"]
                };
            }
            return null;
        }
    }
}