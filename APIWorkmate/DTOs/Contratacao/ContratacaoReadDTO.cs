using APIWorkmate.DTOs.Servico;
using APIWorkmate.DTOs.Usuario;
namespace APIWorkmate.DTOs.Contratacao;

public class ContratacaoReadDTO
{
    public int Id { get; set; }
    public DateTime DataContratacao { get; set; }
    public string? Status { get; set; }

    public UsuarioReadDTO Cliente { get; set; } = default!;
    public ServicoReadDTO Servico { get; set; } = default!;
}
