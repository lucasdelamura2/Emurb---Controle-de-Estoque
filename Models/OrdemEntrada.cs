namespace EmurbEstoque.Models;

public class OrdemEntrada
{
    public int IdOrdEnt { get; set; }
    public int IdFornecedor { get; set; }
    public DateTime DataEnt { get; set; } = DateTime.Now; 
    public string Status { get; set; } = "Aberta";
}
