namespace EmurbEstoque.Models;

public class Funcionario
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string CPF { get; set; } = "";
    public DateTime DataNascimento { get; set; }
}