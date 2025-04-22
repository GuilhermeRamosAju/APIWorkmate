namespace APIWorkmate.DTOs.Servico;

public class ServicoCreateDTO
{
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int CategoriaId { get; set; }
    public int PrestadorId { get; set; }
}
