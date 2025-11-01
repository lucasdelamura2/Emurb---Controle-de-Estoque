namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;

public class Produto
{
    public int IdProduto { get; set; }

    [Display(Name = "Nome")] 
    [Required(ErrorMessage = "O campo Nome é obrigatório.")] 
    [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = "";

    [Display(Name = "Descrição")] 
    [StringLength(255, ErrorMessage = "A Descrição deve ter no máximo 255 caracteres.")] 
    public string? Descricao { get; set; }
}