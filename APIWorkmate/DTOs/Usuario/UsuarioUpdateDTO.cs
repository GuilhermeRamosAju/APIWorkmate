using APIWorkmate.Enums;

namespace APIWorkmate.DTOs.Usuario;

public class UsuarioUpdateDTO
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public TipoUsuario? Tipo { get; set; }
    public string? FotoPerfil { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Disponibilidade { get; set; }
    public string? Formacao { get; set; }
    public string? Experiencia { get; set; }
    public string? Senha { get; set; }
}

