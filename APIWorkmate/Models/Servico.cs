using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIWorkmate.Models;

public class Servico
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public decimal Preco { get; set; }

    [StringLength(150)]
    public string? Localizacao { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    [ForeignKey("Prestador")]
    public Guid PrestadorId { get; set; }
    public Usuario? Prestador { get; set; }

    [ForeignKey("Subcategoria")]
    public Guid SubcategoriaId { get; set; }

    public Subcategoria? Subcategoria { get; set; }

    [JsonIgnore]
    public ICollection<Contratacao>? Contratacoes { get; set; }
    [JsonIgnore]
    public ICollection<Avaliacao>? Avaliacoes { get; set; }
}