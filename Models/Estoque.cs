namespace EmurbEstoque.Models;

public class Estoque
{
    public int IdEstoque { get; set; }
    public int ProdutoId { get; set; }
    public int QuantidadeAtual { get; set; } = 0;
}
