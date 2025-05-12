using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIWorkmate.Models;

public class Avaliacao
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Range(1, 5)]
    public int Nota { get; set; }

    [StringLength(1000)]
    public string? Comentario { get; set; }

    public DateTime DataAvaliacao { get; set; } = DateTime.UtcNow;

    [ForeignKey("Servico")]
    public Guid ServicoId { get; set; }
    public Servico? Servico { get; set; }

    [ForeignKey("Cliente")]
    public Guid ClienteId { get; set; }

    [JsonIgnore]
    public Usuario? Cliente { get; set; }
}