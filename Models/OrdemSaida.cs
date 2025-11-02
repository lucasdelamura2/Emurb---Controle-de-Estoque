namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;
using EmurbEstoque.Models.Validations;

public class OrdemSaida
{
    public int IdOrdSai { get; set; }

    [Display(Name = "Funcionário")]
    [Required(ErrorMessage = "O campo Funcionário é obrigatório.")]
    public int IdFuncionario { get; set; }

    [Display(Name = "Autorização")]
    [Required(ErrorMessage = "O campo Autorização é obrigatório.")]
    public int AutorizaId { get; set; }

    [Display(Name = "Data de Saída")]
    [Required(ErrorMessage = "A data de saída é obrigatória.")]
    [DataType(DataType.Date)]
    [DataMinima("01-01-2025", ErrorMessage = "A data de saída não pode ser anterior a 01/01/2025.")]
    public DateTime DataSaida { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Aberta";
}
