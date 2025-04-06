using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIWorkmate.Models;

public class Contratacao
{
    [Key]
    public int Id { get; set; }

    public DateTime DataContratacao { get; set; } = DateTime.UtcNow;

    [Required, StringLength(50)]
    public string Status { get; set; } = string.Empty;

    [ForeignKey("Servico")]
    public int ServicoId { get; set; }
    public Servico? Servico { get; set; }

    [ForeignKey("Cliente")]
    public int ClienteId { get; set; }
    public Usuario? Cliente { get; set; }
}