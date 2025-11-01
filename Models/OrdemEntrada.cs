namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;

public class OrdemEntrada
{
    public int IdOrdEnt { get; set; }
    
    [Display(Name = "Fornecedor")]
    [Required(ErrorMessage = "O campo Fornecedor é obrigatório.")]
    public int IdFornecedor { get; set; }
    
    [Display(Name = "Data")] 
    [Required(ErrorMessage = "O campo Data é obrigatório.")]
    public DateTime DataEnt { get; set; } = DateTime.Now; 
    public string Status { get; set; } = "Aberta";
}
