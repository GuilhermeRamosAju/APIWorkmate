using APIWorkmate.Enums;

namespace APIWorkmate.DTOs.Usuario;

public class UsuarioReadDTO
{

    public Guid? Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public TipoUsuario Tipo { get; set; }
    public string? FotoPerfil { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Disponibilidade { get; set; }
    public string? Formacao { get; set; }
    public string? Experiencia { get; set; }
}
