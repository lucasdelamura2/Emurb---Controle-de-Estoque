namespace EmurbEstoque.Models;

public class Pessoa
{
    public int IdPessoa { get; set; }
    public string Nome { get; set; } = "";
    public string CpfCnpj { get; set; } = "";
    public string Email { get; set; } = "";
    public string Telefone { get; set; } = "";
}
