using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models
{
    public class Autorizacao
    {
        [Key]
        public int IdAutoriza { get; set; }

        [Required(ErrorMessage = "Selecione a função.")]
        [Display(Name = "Função Autorizada")]
        public int AutorizadoId { get; set; }

        [Required(ErrorMessage = "Selecione o local.")]
        [Display(Name = "Local de Destino")]
        public int LocalId { get; set; }
    }
}