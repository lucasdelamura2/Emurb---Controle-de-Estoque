using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models
{
    public class Autorizado
    {
        [Key]
        public int IdAutorizado { get; set; }

        [Required(ErrorMessage = "A função é obrigatória.")]
        [StringLength(50)]
        [Display(Name = "Função / Cargo Autorizado")]
        public string Funcao { get; set; } = "";
    }
}