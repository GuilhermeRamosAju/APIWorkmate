namespace APIWorkmate.DTOs.Contratacao;

public class ContratacaoCreateDTO
{
    public Guid ClienteId { get; set; }
    public Guid ServicoId { get; set; }
    public DateTime DataContratacao { get; set; }
    public string? Status { get; set; }
}
