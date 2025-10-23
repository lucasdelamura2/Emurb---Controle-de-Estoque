namespace EmurbEstoque.Models;

public class OrdemSaida
{
    public int IdOrdSai { get; set; }
    public int FuncId { get; set; }
    public int AutorizaId { get; set; }
    public DateTime DataSaida { get; set; } = DateTime.Now;
}
