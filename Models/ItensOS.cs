using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models
{
    public class ItensOS
    {
        // Adicionamos um ID primário simples para facilitar o CRUD
        [Key]
        public int IdItemOS { get; set; }

        [Required]
        public int OrdSaiId { get; set; } // FK para OrdemSaida

        [Required(ErrorMessage = "Selecione o lote de origem.")]
        [Display(Name = "Lote de Origem")]
        public int LoteId { get; set; } // FK para Lote

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        [Display(Name = "Quantidade a Retirar")]
        public int Qtd { get; set; }
    }
}