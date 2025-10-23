namespace EmurbEstoque.Models;

public class OrdemEntrada
{
    public int IdOrdEnt { get; set; }
    public int FornId { get; set; }
    public DateTime DataEnt { get; set; } = DateTime.Now; 
}
