using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIWorkmate.Models;

public class Mensagem
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(1000)]
    public string Texto { get; set; } = string.Empty;

    public DateTime DataEnvio { get; set; } = DateTime.UtcNow;

    public bool Lida { get; set; } = false;

    [ForeignKey("Remetente")]
    public Guid RemetenteId { get; set; }
    public Usuario? Remetente { get; set; }

    [ForeignKey("Destinatario")]
    public Guid DestinatarioId { get; set; }

    [JsonIgnore]
    public Usuario? Destinatario { get; set; }
}