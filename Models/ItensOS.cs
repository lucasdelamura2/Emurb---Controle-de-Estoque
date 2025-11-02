namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations; 

public class ItensOS
{
    public int IdItemOS { get; set; }
    public int OrdSaiId { get; set; }

    [Display(Name = "Lote de Origem")]
    [Required(ErrorMessage = "É obrigatório selecionar um Lote.")]
    public int LoteId { get; set; } 

    [Display(Name = "Quantidade Retirada")]
    [Required(ErrorMessage = "O campo Quantidade é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "A Quantidade deve ser maior que zero.")]
    public int Qtd { get; set; } 
}