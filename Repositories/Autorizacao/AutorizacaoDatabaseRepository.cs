using EmurbEstoque.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EmurbEstoque.Repositories
{
    public class AutorizacaoDatabaseRepository : IAutorizacaoRepository
    {
        private readonly string _connectionString;

        public AutorizacaoDatabaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Autorizacao autorizacao)
        {
            if (autorizacao == null)
                throw new ArgumentNullException(nameof(autorizacao));

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                if (Exists(autorizacao.AutorizadoId, autorizacao.LocalId))
                {
                    Console.WriteLine($"Aviso: Tentativa de adicionar autorização duplicada para AutorizadoId={autorizacao.AutorizadoId}, LocalId={autorizacao.LocalId}");
                    return;
                }

                var command = new SqlCommand(
                    "INSERT INTO Autorizacao (autorizadoId, localId) VALUES (@autorizadoId, @localId)",
                    connection
                );

                command.Parameters.AddWithValue("@autorizadoId", autorizacao.AutorizadoId);
                command.Parameters.AddWithValue("@localId", autorizacao.LocalId);

                command.ExecuteNonQuery();
            }
        }

        public List<Autorizacao> GetAll()
        {
            var autorizacoes = new List<Autorizacao>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT idAutoriza, autorizadoId, localId FROM Autorizacao", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        autorizacoes.Add(new Autorizacao
                        {
                            IdAutoriza = reader.GetInt32(0),
                            AutorizadoId = reader.GetInt32(1),
                            LocalId = reader.GetInt32(2)
                        });
                    }
                }
            }

            return autorizacoes;
        }

        public Autorizacao? GetById(int id)
        {
            Autorizacao? autorizacao = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(
                    "SELECT idAutoriza, autorizadoId, localId FROM Autorizacao WHERE idAutoriza = @id",
                    connection
                );

                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        autorizacao = new Autorizacao
                        {
                            IdAutoriza = reader.GetInt32(0),
                            AutorizadoId = reader.GetInt32(1),
                            LocalId = reader.GetInt32(2)
                        };
                    }
                }
            }

            return autorizacao;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("DELETE FROM Autorizacao WHERE idAutoriza = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }

        public bool Exists(int autorizadoId, int localId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(
                    "SELECT COUNT(*) FROM Autorizacao WHERE autorizadoId = @autorizadoId AND localId = @localId",
                    connection
                );

                command.Parameters.AddWithValue("@autorizadoId", autorizadoId);
                command.Parameters.AddWithValue("@localId", localId);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        public void Update(Autorizacao autorizacao)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var checkCommand = new SqlCommand(
                    "SELECT COUNT(*) FROM Autorizacao WHERE autorizadoId = @autorizadoId AND localId = @localId AND idAutoriza != @id",
                    connection
                );
                checkCommand.Parameters.AddWithValue("@autorizadoId", autorizacao.AutorizadoId);
                checkCommand.Parameters.AddWithValue("@localId", autorizacao.LocalId);
                checkCommand.Parameters.AddWithValue("@id", autorizacao.IdAutoriza);

                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    throw new InvalidOperationException("Esta combinação de Função e Local já existe.");
                }

                var command = new SqlCommand(
                    @"UPDATE Autorizacao 
                    SET autorizadoId = @autorizadoId, 
                        localId = @localId 
                    WHERE idAutoriza = @id",
                    connection
                );

                command.Parameters.AddWithValue("@id", autorizacao.IdAutoriza);
                command.Parameters.AddWithValue("@autorizadoId", autorizacao.AutorizadoId);
                command.Parameters.AddWithValue("@localId", autorizacao.LocalId);

                command.ExecuteNonQuery();
            }
        }
    }
}
