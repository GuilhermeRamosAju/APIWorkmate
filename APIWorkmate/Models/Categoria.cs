using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIWorkmate.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Servico>? Servicos { get; set; }

    [JsonIgnore]
    public ICollection<Subcategoria>? Subcategorias { get; set; }
}