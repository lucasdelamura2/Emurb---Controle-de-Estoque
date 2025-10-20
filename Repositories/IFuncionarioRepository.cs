namespace EmurbEstoque.Repositories;

using EmurbEstoque.Models;

public interface IFuncionarioRepository
{
    void Create(Funcionario funcionario);
    List<Funcionario> Read();
    Funcionario? Read(int id);
    void Update(Funcionario funcionario);
    void Delete(int id);
}