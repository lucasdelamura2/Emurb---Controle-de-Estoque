using EmurbEstoque.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmurbEstoque.Repositories
{
    public class UsuarioMemoryRepository : IUsuarioRepository
    {
        private static readonly List<Usuario> _usuariosMem = new();
        private static int _nextId = 1;
        public Usuario? Create(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "Objeto Usuário não pode ser nulo.");
            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new ArgumentException("E-mail do usuário não pode ser vazio.", nameof(usuario.Email));
             if (string.IsNullOrWhiteSpace(usuario.Senha)) 
                throw new ArgumentException("Senha do usuário não pode ser vazia.", nameof(usuario.Senha));
            if (EmailExists(usuario.Email))
            {
                Console.WriteLine($"Erro: E-mail '{usuario.Email}' já cadastrado.");
                return null;
            }
            usuario.IdUsuario = _nextId++;
            _usuariosMem.Add(usuario);
            return usuario;
        }
        public Usuario? ValidateCredentials(string email, string senhaPura)
        {
            var usuario = GetByEmail(email);
            if (usuario == null || !usuario.IsAtivo)
            {
                return null; 
            }
            bool senhaValida = usuario.Senha == senhaPura;
            return senhaValida ? usuario : null;
        }
        public Usuario? GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return _usuariosMem.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
        public Usuario? GetById(int id)
        {
             return _usuariosMem.FirstOrDefault(u => u.IdUsuario == id);
        }
         public bool EmailExists(string email)
         {
             if (string.IsNullOrWhiteSpace(email)) return false;
             return _usuariosMem.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
         }
        public List<Usuario> GetAll()
        {
            return _usuariosMem.OrderBy(u => u.Email).ToList();
        }
        public void Update(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));
            var usuarioExistente = GetById(usuario.IdUsuario);
            if (usuarioExistente == null)
            {
                Console.WriteLine($"Erro: Usuário com ID {usuario.IdUsuario} não encontrado para atualização.");
                return;
            }
            bool emailMudou = !usuarioExistente.Email.Equals(usuario.Email, StringComparison.OrdinalIgnoreCase);
            if (emailMudou && _usuariosMem.Any(u => u.IdUsuario != usuario.IdUsuario && u.Email.Equals(usuario.Email, StringComparison.OrdinalIgnoreCase)))
            {
                 throw new InvalidOperationException($"O e-mail '{usuario.Email}' já está sendo usado por outro usuário.");
            }
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.FuncionarioId = usuario.FuncionarioId;
            usuarioExistente.IsAdmin = usuario.IsAdmin; 
            usuarioExistente.IsAtivo = usuario.IsAtivo;
            if (usuario.Senha != null)
            {
                usuarioExistente.Senha = usuario.Senha;
            }
        }
        public void Delete(int id)
        {
            var usuarioParaDeletar = GetById(id);
            if (usuarioParaDeletar != null)
            {
                _usuariosMem.Remove(usuarioParaDeletar); 
            }
        }
    }
}