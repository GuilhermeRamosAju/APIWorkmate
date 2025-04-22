namespace APIWorkmate.DTOs.Avaliacao;

public class AvaliacaoReadDTO
{
    public int Id { get; set; }
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime DataAvaliacao { get; set; }
    public string? NomeCliente { get; set; }
}
