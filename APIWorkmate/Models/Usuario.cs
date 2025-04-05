using System.ComponentModel.DataAnnotations;

namespace APIWorkmate.Models;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string SenhaHash { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    [Required]
    public string Tipo { get; set; } = string.Empty;

    public string? FotoPerfil { get; set; }

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public ICollection<Servico>? Servicos { get; set; }
    public ICollection<Contratacao>? Contratacoes { get; set; }
    public ICollection<Avaliacao>? Avaliacoes { get; set; }
    public ICollection<Mensagem>? MensagensEnviadas { get; set; }
    public ICollection<Mensagem>? MensagensRecebidas { get; set; }
}
