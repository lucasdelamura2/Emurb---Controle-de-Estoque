namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;

public class Local
{
    public int IdLocal { get; set; }

    [Display(Name = "Nome")]
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(50, ErrorMessage = "O Nome deve ter no máximo 50 caracteres.")]
    public string Nome { get; set; } = "";
    
    [Display(Name = "Descrição")] 
    [Required(ErrorMessage = "O campo Descrição é obrigatório.")] 
    [StringLength(250, ErrorMessage = "O Descrição deve ter no máximo 250 caracteres.")]
    public string Descricao { get; set; } = "";
}
