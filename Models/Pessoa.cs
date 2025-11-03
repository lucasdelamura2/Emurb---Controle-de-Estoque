namespace EmurbEstoque.Models;
using System.ComponentModel.DataAnnotations;

public class Pessoa
    {
        public int IdPessoa { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(75, ErrorMessage = "O Nome deve ter no máximo 75 caracteres.")] //
        public string Nome { get; set; } = "";

        [Display(Name = "CPF/CNPJ")]
        [Required(ErrorMessage = "O campo CPF/CNPJ é obrigatório.")]
        [StringLength(20, ErrorMessage = "O CPF/CNPJ deve ter no máximo 20 caracteres.")] //
        public string CpfCnpj { get; set; } = "";

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
        [StringLength(50, ErrorMessage = "O E-mail deve ter no máximo 50 caracteres.")] //
        public string Email { get; set; } = "";

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [StringLength(20, ErrorMessage = "O Telefone deve ter no máximo 20 caracteres.")] //
        public string Telefone { get; set; } = "";
    }
