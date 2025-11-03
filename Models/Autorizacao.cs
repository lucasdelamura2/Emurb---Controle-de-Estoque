namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;

public class Autorizacao
{
    public int IdAutoriza { get; set; }

    [Display(Name = "Cargo")]
    [Required(ErrorMessage = "O campo Cargo é obrigatório.")]
    public int AutorizadoId { get; set; }
    
    [Display(Name = "Local")] 
    [Required(ErrorMessage = "O campo Local é obrigatório.")]
    public int LocalId { get; set; }
}
