namespace APIWorkmate.DTOs.Servico;
public class ServicoReadDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public decimal? Preco { get; set; }
    public string? SubcategoriaNome { get; set; } = null!;
    public string? PrestadorNome { get; set; } = null!;
}
