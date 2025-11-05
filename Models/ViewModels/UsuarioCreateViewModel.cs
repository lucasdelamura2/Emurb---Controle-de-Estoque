using System.ComponentModel.DataAnnotations;
using EmurbEstoque.Models; 

namespace EmurbEstoque.Models.ViewModels
{
    public class UsuarioCreateViewModel : Usuario
    {
        [Display(Name = "Confirmar Senha")]
        [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; } = "";
    }
}