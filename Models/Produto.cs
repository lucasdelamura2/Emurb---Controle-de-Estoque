using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models
{
    public class Produto
    {
        [Key]
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; } = "";

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(100)]
        public string Descricao { get; set; } = "";
    }
}