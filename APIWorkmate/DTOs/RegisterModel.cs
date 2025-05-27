using APIWorkmate.Enums;
using System.ComponentModel.DataAnnotations;

namespace APIWorkmate.DTOs;

public class RegisterModel
{
    [Required(ErrorMessage = "Nome de usuário é necessário")]
    public required string? UserName { get; set; }

    [Required(ErrorMessage = "Email de usuário é necessário")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Senha é neccesária")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Tipo do usuário é neccesário, escolha entre Cliente ou Prestador"), EnumDataType(typeof(TipoUsuario))]
    public required TipoUsuario TipoUsuario { get; set; }

    public string? Nome { get; set; } = string.Empty;

    public string? FotoPerfil { get; set; }

    public string? Cidade { get; set; }

    public string? Estado { get; set; }

    public string? Disponibilidade { get; set; }

    public string? Formacao { get; set; }

    public string? Experiencia{ get; set; }
}
