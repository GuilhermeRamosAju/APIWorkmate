using System.ComponentModel.DataAnnotations;

namespace APIWorkmate.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Nome de usuário é necessário")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Senha é neccesária")]
    public string? Password { get; set; }
}
