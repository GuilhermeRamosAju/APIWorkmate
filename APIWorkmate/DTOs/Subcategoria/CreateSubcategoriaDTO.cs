using System.ComponentModel.DataAnnotations;

namespace APIWorkmate.DTOs.Subcategoria;

public class CreateSubcategoriaDTO
{
    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public int CategoriaId { get; set; }
}
