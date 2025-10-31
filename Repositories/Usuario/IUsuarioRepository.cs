using EmurbEstoque.Models;
using System.Collections.Generic;

namespace EmurbEstoque.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario? Create(Usuario usuario);
        Usuario? ValidateCredentials(string email, string senhaPura);
        Usuario? GetByEmail(string email);
        Usuario? GetById(int id);
        bool EmailExists(string email);
        List<Usuario> GetAll();
        void Update(Usuario usuario); 
        void Delete(int id); 
    }
}