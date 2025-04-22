using APIWorkmate.Enums;
using System.ComponentModel.DataAnnotations;

namespace APIWorkmate.DTOs.Usuario;

public class UsuarioCreateDTO
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string SenhaHash { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefone { get; set; }

    [Required]
    [EnumDataType(typeof(TipoUsuario))]
    public TipoUsuario Tipo { get; set; }

    [StringLength(2083)]
    [Url]
    public string? FotoPerfil { get; set; }

    [StringLength(100)]
    public string? Cidade { get; set; }

    [StringLength(2)]
    public string? Estado { get; set; }

    [StringLength(100)]
    public string? Disponibilidade { get; set; }

    [StringLength(255)]
    public string? Formacao { get; set; }

    [StringLength(255)]
    public string? Experiencia { get; set; }
}
