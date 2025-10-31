using EmurbEstoque.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace EmurbEstoque.Repositories
{
    public class EstoqueDatabaseRepository : IEstoqueRepository
    {
        private readonly string _connectionString;
        private readonly IProdutoRepository _produtoRepo;
        private readonly ILoteRepository _loteRepo;
        private readonly IItensOSRepository _itensOSRepo;

        public EstoqueDatabaseRepository(
            string connectionString,
            IProdutoRepository produtoRepository,
            ILoteRepository loteRepository,
            IItensOSRepository itensOSRepository)
        {
            _connectionString = connectionString;
            _produtoRepo = produtoRepository;
            _loteRepo = loteRepository;
            _itensOSRepo = itensOSRepository;
        }

        public List<Estoque> GetEstoqueConsolidado()
        {
            var todosProdutos = _produtoRepo.GetAll();
            var todasEntradas = _loteRepo.GetAll();
            var todasSaidas = _itensOSRepo.GetAll();

            var estoqueConsolidado = new List<Estoque>();

            foreach (var produto in todosProdutos)
            {
                int totalEntrada = todasEntradas
                    .Where(l => l.ProdutoId == produto.IdProduto)
                    .Sum(l => l.Qtd);

                var lotesDoProdutoIds = todasEntradas
                    .Where(l => l.ProdutoId == produto.IdProduto)
                    .Select(l => l.IdLote)
                    .ToList();

                int totalSaida = todasSaidas
                    .Where(s => lotesDoProdutoIds.Contains(s.LoteId))
                    .Sum(s => s.Qtd);

                int saldoAtual = totalEntrada - totalSaida;

                UpsertEstoque(produto.IdProduto, saldoAtual);

                estoqueConsolidado.Add(new Estoque
                {
                    ProdutoId = produto.IdProduto,
                    NomeProduto = produto.Nome,
                    QtdEntrada = totalEntrada,
                    QtdSaida = totalSaida,
                    SaldoAtual = saldoAtual
                });
            }

            return estoqueConsolidado.OrderBy(p => p.NomeProduto).ToList();
        }

        public List<Estoque> GetAll()
        {
            var lista = new List<Estoque>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(@"
                    SELECT e.idEstoque, e.produtoId, e.quantidadeAtual, p.nome
                    FROM Estoque e
                    INNER JOIN Produtos p ON p.idProduto = e.produtoId
                    ORDER BY p.nome", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Estoque
                        {
                            IdEstoque = reader.GetInt32(0),
                            ProdutoId = reader.GetInt32(1),
                            SaldoAtual = reader.GetInt32(2),
                            NomeProduto = reader.GetString(3)
                        });
                    }
                }
            }

            return lista;
        }

        public Estoque? GetByProdutoId(int produtoId)
        {
            Estoque? estoque = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(@"
                    SELECT idEstoque, produtoId, quantidadeAtual 
                    FROM Estoque WHERE produtoId = @produtoId", connection);
                command.Parameters.AddWithValue("@produtoId", produtoId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        estoque = new Estoque
                        {
                            IdEstoque = reader.GetInt32(0),
                            ProdutoId = reader.GetInt32(1),
                            SaldoAtual = reader.GetInt32(2)
                        };
                    }
                }
            }

            return estoque;
        }

        private void UpsertEstoque(int produtoId, int quantidadeAtual)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var checkCommand = new SqlCommand(
                    "SELECT COUNT(*) FROM Estoque WHERE produtoId = @produtoId",
                    connection);
                checkCommand.Parameters.AddWithValue("@produtoId", produtoId);

                int count = (int)checkCommand.ExecuteScalar();

                SqlCommand command;
                if (count > 0)
                {
                    command = new SqlCommand(
                        "UPDATE Estoque SET quantidadeAtual = @quantidadeAtual WHERE produtoId = @produtoId",
                        connection);
                }
                else
                {
                    command = new SqlCommand(
                        "INSERT INTO Estoque (produtoId, quantidadeAtual) VALUES (@produtoId, @quantidadeAtual)",
                        connection);
                }

                command.Parameters.AddWithValue("@produtoId", produtoId);
                command.Parameters.AddWithValue("@quantidadeAtual", quantidadeAtual);
                command.ExecuteNonQuery();
            }
        }
    }
}
