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

    [Required, StringLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [StringLength(2083)]
    public string? FotoPerfil { get; set; }

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

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