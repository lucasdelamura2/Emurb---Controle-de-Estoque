namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;
using EmurbEstoque.Models.Validations;

public class Lote
{
    public int IdLote { get; set; }

    [Display(Name = "Produto")]
    [Required(ErrorMessage = "O campo Produto é obrigatório.")]
    public int ProdutoId { get; set; }
    public int OrdEntId { get; set; }

    [Display(Name = "Quantidade")]
    [Required(ErrorMessage = "O campo Quantidade é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "A Quantidade deve ser maior que zero.")]
    public int Qtd { get; set; }

    [Display(Name = "Preço")]
    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O Preço deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [Display(Name = "Data de Validade")]
    [Required(ErrorMessage = "A data de validade é obrigatória.")]
    [DataType(DataType.Date)]
    [DataMinima("01-01-2025", ErrorMessage = "A data de validade não pode ser anterior a 01/01/2025.")]
    public DateTime? DataValidade { get; set; }
}