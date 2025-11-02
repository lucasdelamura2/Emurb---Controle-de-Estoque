namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;

public class Funcionario : Pessoa
{   
    public int IdFuncionario { get { return IdPessoa; } set { IdPessoa = value; } }

    [Display(Name = "Cargo")]
    [Required(ErrorMessage = "O campo Cargo é obrigatório.")]
    [StringLength(50, ErrorMessage = "O Cargo deve ter no máximo 50 caracteres.")]
    public string Cargo { get; set; } = "";
    
    [Display(Name = "Setor")] 
    [Required(ErrorMessage = "O campo Setor é obrigatório.")] 
    [StringLength(50, ErrorMessage = "O Setor deve ter no máximo 50 caracteres.")]
    public string Setor { get; set; } = "";
}
