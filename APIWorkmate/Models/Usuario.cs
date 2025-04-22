using APIWorkmate.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIWorkmate.Models;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string SenhaHash { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefone { get; set; }

    [Required, EnumDataType(typeof(TipoUsuario))]
    public TipoUsuario Tipo { get; set; }

    [StringLength(2083)]
    public string? FotoPerfil { get; set; }

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

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

    [JsonIgnore]
    public ICollection<Servico>? Servicos { get; set; }
    [JsonIgnore]
    public ICollection<Contratacao>? Contratacoes { get; set; }
    [JsonIgnore]
    public ICollection<Avaliacao>? Avaliacoes { get; set; }
    [JsonIgnore]
    public ICollection<Mensagem>? MensagensEnviadas { get; set; }
    [JsonIgnore]
    public ICollection<Mensagem>? MensagensRecebidas { get; set; }
}