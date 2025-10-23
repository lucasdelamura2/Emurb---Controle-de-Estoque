namespace EmurbEstoque.Models;

public class Lote
{
    public int IdLote { get; set; }
    public int ProdutoId { get; set; }
    public int OrdEntId { get; set; }
    public int Qtd { get; set; }
    public decimal Preco { get; set; }
    public DateTime? DataValidade { get; set; }
}
