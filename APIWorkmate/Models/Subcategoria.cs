using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIWorkmate.Models;

public class Subcategoria
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [ForeignKey("Categoria")]
    public Guid CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }

    [JsonIgnore]
    public ICollection<Usuario>? Usuarios { get; set; }

    [JsonIgnore]
    public ICollection<Servico>? Servicos { get; set; }
}
