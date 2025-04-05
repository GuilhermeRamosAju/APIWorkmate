using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIWorkmate.Models;

public class Mensagem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Texto { get; set; } = string.Empty;

    public DateTime DataEnvio { get; set; } = DateTime.UtcNow;

    public bool Lida { get; set; } = false;

    [ForeignKey("Remetente")]
    public int RemetenteId { get; set; }
    public Usuario? Remetente { get; set; }

    [ForeignKey("Destinatario")]
    public int DestinatarioId { get; set; }
    public Usuario? Destinatario { get; set; }
}
