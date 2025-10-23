namespace EmurbEstoque.Repositories;

using EmurbEstoque.Models;

public class FornecedorMemoryRepository : IFornecedorRepository
{
    private readonly List<Fornecedor> lista = new();

    public void Create(Fornecedor fornecedor)
    {
        fornecedor.Id = Math.Abs((int)DateTimeOffset.Now.ToUnixTimeMilliseconds());
        lista.Add(fornecedor);
    }

    public List<Fornecedor> Read() => lista;

    public Fornecedor? Read(int id) => lista.SingleOrDefault(e => e.Id == id);

    public void Update(Fornecedor fornecedor)
    {
        var f = lista.SingleOrDefault(e => e.Id == fornecedor.Id);
        if (f is null) return;

        f.Nome = fornecedor.Nome;
        f.CpfCnpj = fornecedor.CpfCnpj;     
        f.Email = fornecedor.Email;       
        f.Telefone = fornecedor.Telefone;   
        f.InscricaoEstadual = fornecedor.InscricaoEstadual;         
    }

    public void Delete(int id)
    {
        var f = lista.SingleOrDefault(e => e.Id == id);
        if (f is null) return;
        lista.Remove(f);
    }
}