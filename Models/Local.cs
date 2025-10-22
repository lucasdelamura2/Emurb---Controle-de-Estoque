using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models
{
    public class Local
    {
        [Key]
        public int IdLocal { get; set; }

        [Required(ErrorMessage = "O nome do local é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; } = "";

        [StringLength(100)]
        public string Descricao { get; set; } = "";
    }
}