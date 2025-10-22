using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models
{
    public class OrdemSaida
    {
        [Key]
        public int IdOrdSai { get; set; }

        [Required(ErrorMessage = "Selecione o funcionário requisitante.")]
        [Display(Name = "Funcionário Requisitante")]
        public int FuncId { get; set; }

        [Required(ErrorMessage = "Selecione a autorização (Função -> Local).")]
        [Display(Name = "Autorização (Função -> Local)")]
        public int AutorizaId { get; set; }

        [Required]
        [Display(Name = "Data da Saída")]
        public DateTime DataSaida { get; set; } = DateTime.Now;
    }
}