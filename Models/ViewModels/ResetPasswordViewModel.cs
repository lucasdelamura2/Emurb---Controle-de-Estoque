using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } 

        [Display(Name = "Nova Senha")]
        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "A senha deve ter entre 4 e 25 caracteres.")]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; } = "";

        [Display(Name = "Confirmar Nova Senha")]
        [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; } = "";
    }
}