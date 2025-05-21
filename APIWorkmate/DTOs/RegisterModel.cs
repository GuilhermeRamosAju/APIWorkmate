using System.ComponentModel.DataAnnotations;

namespace APIWorkmate.DTOs;

public class RegisterModel
{
    [Required(ErrorMessage = "Nome de usuário é necessário")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Email de usuário é necessário")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Senha é neccesária")]
    public string? Password { get; set; }
}
