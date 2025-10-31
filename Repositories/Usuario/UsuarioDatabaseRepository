using EmurbEstoque.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public class UsuarioDatabaseRepository : DbConnection, IUsuarioRepository
    {
        public UsuarioDatabaseRepository(string connStr) : base(connStr) { }

        public Usuario? Create(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (EmailExists(usuario.Email))
                throw new InvalidOperationException($"E-mail '{usuario.Email}' já está cadastrado.");

            string sql = @"
                INSERT INTO Usuarios (email, senha, funcionarioId, isAdmin, isAtivo)
                OUTPUT INSERTED.idUsuario
                VALUES (@Email, @Senha, @FuncionarioId, @IsAdmin, @IsAtivo);
            ";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@Senha", usuario.Senha);
                cmd.Parameters.AddWithValue("@FuncionarioId", (object?)usuario.FuncionarioId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsAdmin", usuario.IsAdmin);
                cmd.Parameters.AddWithValue("@IsAtivo", usuario.IsAtivo);

                usuario.IdUsuario = (int)cmd.ExecuteScalar();
            }

            return usuario;
        }

        public Usuario? ValidateCredentials(string email, string senhaPura)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senhaPura))
                return null;

            string sql = "SELECT * FROM Usuarios WHERE email = @Email AND isAtivo = 1";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string senhaBanco = reader["senha"].ToString()!;
                        if (senhaBanco == senhaPura)
                        {
                            return Map(reader);
                        }
                    }
                }
            }

            return null;
        }

        public Usuario? GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            string sql = "SELECT * FROM Usuarios WHERE email = @Email";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? Map(reader) : null;
                }
            }
        }

        public Usuario? GetById(int id)
        {
            string sql = "SELECT * FROM Usuarios WHERE idUsuario = @Id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? Map(reader) : null;
                }
            }
        }

        public bool EmailExists(string email)
        {
            string sql = "SELECT COUNT(*) FROM Usuarios WHERE email = @Email";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public List<Usuario> GetAll()
        {
            List<Usuario> lista = new();
            string sql = "SELECT * FROM Usuarios ORDER BY email";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(Map(reader));
                }
            }

            return lista;
        }

        public void Update(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            string sql = @"
                UPDATE Usuarios
                SET email = @Email,
                    senha = @Senha,
                    funcionarioId = @FuncionarioId,
                    isAdmin = @IsAdmin,
                    isAtivo = @IsAtivo
                WHERE idUsuario = @IdUsuario;
            ";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@Senha", usuario.Senha);
                cmd.Parameters.AddWithValue("@FuncionarioId", (object?)usuario.FuncionarioId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsAdmin", usuario.IsAdmin);
                cmd.Parameters.AddWithValue("@IsAtivo", usuario.IsAtivo);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string sql = "DELETE FROM Usuarios WHERE idUsuario = @Id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private Usuario Map(SqlDataReader reader)
        {
            return new Usuario
            {
                IdUsuario = Convert.ToInt32(reader["idUsuario"]),
                Email = reader["email"].ToString()!,
                Senha = reader["senha"].ToString()!,
                FuncionarioId = reader["funcionarioId"] == DBNull.Value ? null : Convert.ToInt32(reader["funcionarioId"]),
                IsAdmin = Convert.ToBoolean(reader["isAdmin"]),
                IsAtivo = Convert.ToBoolean(reader["isAtivo"])
            };
        }
    }
}
