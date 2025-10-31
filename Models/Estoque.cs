namespace EmurbEstoque.Models;

public class Estoque
{
    public int IdEstoque { get; set; }
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; }    
    public int QtdEntrada { get; set; }
    public int QtdSaida { get; set; }
    public int SaldoAtual { get; set; }
}
