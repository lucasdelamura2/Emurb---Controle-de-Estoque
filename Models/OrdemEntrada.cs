using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models
{
    public class OrdemEntrada
    {
        [Key]
        public int IdOrdEnt { get; set; }

        [Required(ErrorMessage = "O fornecedor é obrigatório.")]
        [Display(Name = "Fornecedor")]
        public int FornId { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        [Display(Name = "Data da Entrada")]
        public DateTime DataEnt { get; set; } = DateTime.Now;
    }
}