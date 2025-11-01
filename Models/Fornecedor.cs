namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;

public class Fornecedor : Pessoa
{
    public int IdFornecedor { get { return IdPessoa; } set { IdPessoa = value; } }

    [Display(Name = "Inscrição Estadual")] 
    [Required(ErrorMessage = "O campo Inscrição Estadual é obrigatório.")] 
    [StringLength(20, ErrorMessage = "O Inscrição Estadual deve ter no máximo 20 caracteres.")]
    public string InscricaoEstadual { get; set; } = "";
}
