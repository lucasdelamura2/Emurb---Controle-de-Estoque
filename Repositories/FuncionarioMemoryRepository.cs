namespace EmurbEstoque.Repositories;

using EmurbEstoque.Models;

public class FuncionarioMemoryRepository : IFuncionarioRepository
{
    private readonly List<Funcionario> lista = new();

    public void Create(Funcionario funcionario)
    {
        funcionario.Id = Math.Abs((int)DateTimeOffset.Now.ToUnixTimeMilliseconds());
        lista.Add(funcionario);
    }

    public List<Funcionario> Read() => lista;

    public Funcionario? Read(int id) => lista.SingleOrDefault(e => e.Id == id);

    public void Update(Funcionario funcionario)
    {
        var f = lista.SingleOrDefault(e => e.Id == funcionario.Id);
        if (f is null) return;
        f.Nome = funcionario.Nome;
        f.CPF = funcionario.CPF;
        f.Email = funcionario.Email;
        f.Telefone = funcionario.Telefone;
        f.Cargo = funcionario.Cargo;
        f.Setor = funcionario.Setor;
    }

    public void Delete(int id)
    {
        var f = lista.SingleOrDefault(e => e.Id == id);
        if (f is null) return;
        lista.Remove(f);
    }
}