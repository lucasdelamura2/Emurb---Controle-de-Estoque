using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic; 
using System; 

namespace EmurbEstoque.Repositories
{
    public class LoteDatabaseRepository : DbConnection, ILoteRepository
    {
        public LoteDatabaseRepository(string connStr) : base(connStr) { }

        public void Create(Lote lote)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                INSERT INTO Lote (produtoId, OrdEntId, qtd, preco, dataValidade) 
                VALUES (@prodId, @ordEntId, @qtd, @preco, @dataVal)
            ";

            cmd.Parameters.AddWithValue("@prodId", lote.ProdutoId);
            cmd.Parameters.AddWithValue("@ordEntId", lote.OrdEntId);
            cmd.Parameters.AddWithValue("@qtd", lote.Qtd);
            cmd.Parameters.AddWithValue("@preco", lote.Preco);

            if (lote.DataValidade.HasValue)
            {
                cmd.Parameters.AddWithValue("@dataVal", lote.DataValidade.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dataVal", DBNull.Value);
            }

            cmd.ExecuteNonQuery();
        }

        public List<Lote> GetByOrdemEntradaId(int ordemEntradaId)
        {
            var lista = new List<Lote>(); 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Lote WHERE OrdEntId = @ordId";
            cmd.Parameters.AddWithValue("@ordId", ordemEntradaId);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Lote
                    {
                        IdLote = (int)reader["idLote"],
                        ProdutoId = (int)reader["produtoId"],
                        OrdEntId = (int)reader["OrdEntId"],
                        Qtd = (int)reader["qtd"],
                        Preco = (decimal)reader["preco"],
                        DataValidade = reader["dataValidade"] == DBNull.Value ? 
                                        null : 
                                        (DateTime?)reader["dataValidade"]
                    });
                }
            }
            return lista;
        }

        public List<Lote> GetAll()
        {
             var lista = new List<Lote>(); 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Lote";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Lote
                    {
                        IdLote = (int)reader["idLote"],
                        ProdutoId = (int)reader["produtoId"],
                        OrdEntId = (int)reader["OrdEntId"],
                        Qtd = (int)reader["qtd"],
                        Preco = (decimal)reader["preco"],
                        DataValidade = reader["dataValidade"] == DBNull.Value ? 
                                        null : 
                                        (DateTime?)reader["dataValidade"]
                    });
                }
            } 
            return lista;
        }

        public Lote? GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Lote WHERE idLote = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Lote
                    {
                        IdLote = (int)reader["idLote"],
                        ProdutoId = (int)reader["produtoId"],
                        OrdEntId = (int)reader["OrdEntId"],
                        Qtd = (int)reader["qtd"],
                        Preco = (decimal)reader["preco"],
                        DataValidade = reader["dataValidade"] == DBNull.Value ?
                                        null :
                                        (DateTime?)reader["dataValidade"]
                    };
                }
            } 
            return null;
        }
        public void Delete(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM Lote WHERE idLote = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        public void Update(Lote lote)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                UPDATE Lote 
                SET produtoId = @prodId, 
                    OrdEntId = @ordEntId, 
                    qtd = @qtd, 
                    preco = @preco, 
                    dataValidade = @dataVal
                WHERE idLote = @idLote
            ";

            cmd.Parameters.AddWithValue("@idLote", lote.IdLote);
            cmd.Parameters.AddWithValue("@prodId", lote.ProdutoId);
            cmd.Parameters.AddWithValue("@ordEntId", lote.OrdEntId);
            cmd.Parameters.AddWithValue("@qtd", lote.Qtd);
            cmd.Parameters.AddWithValue("@preco", lote.Preco);

            if (lote.DataValidade.HasValue)
            {
                cmd.Parameters.AddWithValue("@dataVal", lote.DataValidade.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dataVal", DBNull.Value);
            }

            cmd.ExecuteNonQuery();
        }
    }
}