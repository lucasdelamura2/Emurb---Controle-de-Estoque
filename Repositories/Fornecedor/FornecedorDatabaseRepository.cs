using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;

namespace EmurbEstoque.Repositories
{
    public class FornecedorDatabaseRepository : DbConnection, IFornecedorRepository
    {
        public FornecedorDatabaseRepository(string connStr) : base(connStr) { }

        public void Create(Fornecedor fornecedor)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                INSERT INTO Pessoas (nome, cpf_cnpj, email, telefone) 
                VALUES (@nome, @cpf_cnpj, @email, @tel);

                DECLARE @newId INT = SCOPE_IDENTITY();
                
                INSERT INTO Fornecedores (idFornecedor, inscricao_estadual)
                VALUES (@newId, @insc_estadual);
            ";

            cmd.Parameters.AddWithValue("@nome", fornecedor.Nome);
            cmd.Parameters.AddWithValue("@cpf_cnpj", fornecedor.CpfCnpj);
            cmd.Parameters.AddWithValue("@email", fornecedor.Email);
            cmd.Parameters.AddWithValue("@tel", fornecedor.Telefone);
            cmd.Parameters.AddWithValue("@insc_estadual", fornecedor.InscricaoEstadual);

            cmd.ExecuteNonQuery();
        }

        public List<Fornecedor> Read()
        {
            var lista = new List<Fornecedor>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            
            cmd.CommandText = @"
                SELECT * FROM Pessoas p
                JOIN Fornecedores f ON p.idPessoa = f.idFornecedor
                ORDER BY p.nome
            ";

            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                lista.Add(new Fornecedor
                {
                    // Dados de Pessoas
                    IdFornecedor = (int)reader["idPessoa"],
                    Nome = (string)reader["nome"],
                    CpfCnpj = (string)reader["cpf_cnpj"],
                    Email = (string)reader["email"],
                    Telefone = (string)reader["telefone"],

                    // Dados de Fornecedores
                    InscricaoEstadual = (string)reader["inscricao_estadual"]
                });
            }
            return lista;
        }

        public Fornecedor? Read(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                SELECT * FROM Pessoas p
                JOIN Fornecedores f ON p.idPessoa = f.idFornecedor
                WHERE p.idPessoa = @id
            ";
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                 return new Fornecedor
                {
                    // Dados de Pessoas
                    IdFornecedor = (int)reader["idPessoa"],
                    Nome = (string)reader["nome"],
                    CpfCnpj = (string)reader["cpf_cnpj"],
                    Email = (string)reader["email"],
                    Telefone = (string)reader["telefone"],

                    // Dados de Fornecedores
                    InscricaoEstadual = (string)reader["inscricao_estadual"]
                };
            }
            return null;
        }

        public void Update(Fornecedor fornecedor)
        {
            // Atualiza as duas tabelas
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                UPDATE Pessoas SET
                    nome = @nome,
                    cpf_cnpj = @cpf_cnpj,
                    email = @email,
                    telefone = @tel
                WHERE idPessoa = @id;

                UPDATE Fornecedores SET
                    inscricao_estadual = @insc_estadual
                WHERE idFornecedor = @id;
            ";

            cmd.Parameters.AddWithValue("@id", fornecedor.IdFornecedor);

            // Parâmetros da Tabela Pessoas
            cmd.Parameters.AddWithValue("@nome", fornecedor.Nome);
            cmd.Parameters.AddWithValue("@cpf_cnpj", fornecedor.CpfCnpj);
            cmd.Parameters.AddWithValue("@email", fornecedor.Email);
            cmd.Parameters.AddWithValue("@tel", fornecedor.Telefone);

            // Parâmetros da Tabela Fornecedores
            cmd.Parameters.AddWithValue("@insc_estadual", fornecedor.InscricaoEstadual);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"
                DELETE FROM Fornecedores WHERE idFornecedor = @id;
                DELETE FROM Pessoas WHERE idPessoa = @id;
            ";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}