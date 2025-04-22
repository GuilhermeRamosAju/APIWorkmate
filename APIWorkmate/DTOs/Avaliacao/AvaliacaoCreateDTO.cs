using System.ComponentModel.DataAnnotations;

namespace APIWorkmate.DTOs.Avaliacao;

public class AvaliacaoCreateDTO
{
    [Required]
    [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5.")]
    public int Nota { get; set; }

    [StringLength(1000, ErrorMessage = "O comentário não pode exceder 1000 caracteres.")]
    public string? Comentario { get; set; }

    [Required(ErrorMessage = "O campo ServicoId é obrigatório.")]
    public int ServicoId { get; set; }

    [Required(ErrorMessage = "O campo ClienteId é obrigatório.")]
    public int ClienteId { get; set; }
}
