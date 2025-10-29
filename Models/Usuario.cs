namespace EmurbEstoque.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string Email { get; set; } = "";
    public string Senha { get; set; } = ""; 
    public int? FuncionarioId { get; set; } 
    public bool IsAdmin { get; set; } = false;
    public bool IsAtivo { get; set; } = true;
}
