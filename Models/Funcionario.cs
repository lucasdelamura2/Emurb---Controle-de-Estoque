using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models
{
    public class Funcionario
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; } = "";

        [Required(ErrorMessage = "O campo CPF/CNPJ é obrigatório.")]
        [StringLength(20)]
        [Display(Name = "CPF/CNPJ")]
        public string CpfCnpj { get; set; } = ""; 

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = ""; 

        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [StringLength(14)]
        public string Telefone { get; set; } = ""; 

        [Required(ErrorMessage = "O campo Cargo é obrigatório.")]
        [StringLength(50)]
        public string Cargo { get; set; } = ""; 

        [Required(ErrorMessage = "O campo Setor é obrigatório.")]
        [StringLength(50)]
        public string Setor { get; set; } = ""; 
    }
}