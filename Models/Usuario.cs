namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    public int IdUsuario { get; set; }

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "A Senha é obrigatória.")]
    [StringLength(25, MinimumLength = 4, ErrorMessage = "A senha deve ter entre 4 e 25 caracteres.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = ""; 

    [Display(Name = "Funcionário Vinculado")]
    [Required(ErrorMessage = "É obrigatório vincular um funcionário.")]
    public int? FuncionarioId { get; set; } 

    [Display(Name = "Administrador?")]
    public bool IsAdmin { get; set; } = false;

    [Display(Name = "Ativo?")]
    public bool IsAtivo { get; set; } = true;
}