namespace EmurbEstoque.Models;

public class Funcionario : Pessoa
{   public int IdFuncionario { get { return IdPessoa; } set { IdPessoa = value; } }
    public string Cargo { get; set; } = "";
    public string Setor { get; set; } = "";
}
