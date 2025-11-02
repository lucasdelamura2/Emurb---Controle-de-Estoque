namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;
using EmurbEstoque.Models.Validations;

public class OrdemEntrada
{
    public int IdOrdEnt { get; set; }
    
    [Display(Name = "Fornecedor")]
    [Required(ErrorMessage = "O campo Fornecedor é obrigatório.")]
    public int IdFornecedor { get; set; }

    [Display(Name = "Data de Entrada")]
    [Required(ErrorMessage = "A data de entrada é obrigatória.")]
    [DataType(DataType.Date)]
    [DataMinima("01-01-2025", ErrorMessage = "A data de entrada não pode ser anterior a 01/01/2025.")]
    public DateTime DataEnt { get; set; } = DateTime.Now; 
    public string Status { get; set; } = "Aberta";
}