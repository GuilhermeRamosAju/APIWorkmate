namespace APIWorkmate.DTOs.Servico;

public class ServicoUpdateDTO
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public decimal? Preco { get; set; }
    public int? SubcategoriaId { get; set; }
    public int? PrestadorId { get; set; }
}

