using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIWorkmate.Models;

public class Contratacao
{
    [Key]
    public Guid Id { get; set; }

    public DateTime DataContratacao { get; set; } = DateTime.UtcNow;

    [Required, StringLength(50)]
    public string Status { get; set; } = string.Empty;

    [ForeignKey("Servico")]
    public Guid ServicoId { get; set; }
    public Servico? Servico { get; set; }

    [ForeignKey("Cliente")]
    public Guid ClienteId { get; set; }

    [JsonIgnore]
    public Usuario? Cliente { get; set; }
}