namespace EmurbEstoque.Models;

public class Fornecedor : Pessoa
{
    public int IdFornecedor { get { return IdPessoa; } set { IdPessoa = value; } }
    public string InscricaoEstadual { get; set; } = "";
}
