namespace APIWorkmate.DTOs.Contratacao;

public class ContratacaoPatchDTO
{
    public int Id { get; set; }
    public int? ClienteId { get; set; }
    public int? ServicoId { get; set; }
    public DateTime? DataContratacao { get; set; }
    public string? Status { get; set; }
}
