using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmurbEstoque.Models
{
    public class Lote
    {
        [Key]
        public int IdLote { get; set; }

        [Required]
        [Display(Name = "Produto")]
        public int ProdutoId { get; set; }

        [Required]
        public int OrdEntId { get; set; } // Chave estrangeira para a OrdemEntrada

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        [Display(Name = "Quantidade")]
        public int Qtd { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Display(Name = "Preço de Custo (Unid.)")]
        [Column(TypeName = "money")]
        public decimal Preco { get; set; }

        [Display(Name = "Data de Validade")]
        public DateTime? DataValidade { get; set; } // '?' permite valor nulo
    }
}