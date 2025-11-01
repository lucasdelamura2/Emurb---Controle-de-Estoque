namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;

public class Autorizado
{
    public int IdAutorizado { get; set; }

    [Display(Name = "Função")] 
    [Required(ErrorMessage = "O campo Função é obrigatório.")] 
    [StringLength(50, ErrorMessage = "O Função deve ter no máximo 50 caracteres.")]
    public string Funcao { get; set; } = "";
}
