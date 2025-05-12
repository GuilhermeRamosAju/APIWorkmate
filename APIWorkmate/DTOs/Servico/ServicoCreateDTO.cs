namespace APIWorkmate.DTOs.Servico;

public class ServicoCreateDTO
{
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public Guid SubcategoriaId { get; set; }
    public Guid PrestadorId { get; set; }
}
