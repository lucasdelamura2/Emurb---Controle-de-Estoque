using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models.ViewModels
{
    public class UsuarioEditViewModel
    {
        public int IdUsuario { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
        [StringLength(100)]
        public string Email { get; set; } = "";

        [Display(Name = "Funcionário Vinculado")]
        [Required(ErrorMessage = "É obrigatório vincular um funcionário.")]
        public int? FuncionarioId { get; set; } 

        [Display(Name = "Administrador?")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Ativo?")]
        public bool IsAtivo { get; set; }

        [Display(Name = "Nova Senha (deixe em branco para não alterar)")]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "A senha deve ter entre 4 e 25 caracteres.")]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [Display(Name = "Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; }
    }
}