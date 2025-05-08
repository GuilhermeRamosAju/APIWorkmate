namespace APIWorkmate.DTOs.Servico;

public class ServicoCreateDTO
{
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int SubcategoriaId { get; set; }
    public int PrestadorId { get; set; }
}
