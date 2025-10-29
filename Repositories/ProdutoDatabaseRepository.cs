using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;

namespace EmurbEstoque.Repositories
{
    public class ProdutoDatabaseRepository : DbConnection, IProdutoRepository
    {
        public ProdutoDatabaseRepository(string connStr) : base(connStr) { }
        public void Create(Produto produto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO Produtos (nome, descricao) VALUES (@nome, @desc)";

            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@desc", produto.Descricao);

            cmd.ExecuteNonQuery();
        }
        public void Delete(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM Produtos WHERE idProduto = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        public List<Produto> GetAll()
        {
            var lista = new List<Produto>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Produtos ORDER BY nome";

            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                lista.Add(new Produto
                {
                    IdProduto = (int)reader["idProduto"],
                    Nome = (string)reader["nome"],
                    Descricao = (string)reader["descricao"]
                });
            }
            return lista;
        }
        public Produto? GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Produtos WHERE idProduto = @id";
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                return new Produto
                {
                    IdProduto = (int)reader["idProduto"],
                    Nome = (string)reader["nome"],
                    Descricao = (string)reader["descricao"]
                };
            }
            return null;
        }
        public void Update(Produto produto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"UPDATE Produtos SET 
                                    nome = @nome, 
                                    descricao = @desc 
                                WHERE idProduto = @id";           
            cmd.Parameters.AddWithValue("@id", produto.IdProduto);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@desc", produto.Descricao);
            cmd.ExecuteNonQuery();
        }
    }
}