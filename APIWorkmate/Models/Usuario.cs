using System.ComponentModel.DataAnnotations;

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

    public ICollection<Servico>? Servicos { get; set; }
    public ICollection<Contratacao>? Contratacoes { get; set; }
    public ICollection<Avaliacao>? Avaliacoes { get; set; }
    public ICollection<Mensagem>? MensagensEnviadas { get; set; }
    public ICollection<Mensagem>? MensagensRecebidas { get; set; }
}