namespace APIWorkmate.DTOs.Contratacao;

public class ContratacaoPatchDTO
{
    public Guid Id { get; set; }
    public Guid? ClienteId { get; set; }
    public Guid? ServicoId { get; set; }
    public DateTime? DataContratacao { get; set; }
    public string? Status { get; set; }
}
