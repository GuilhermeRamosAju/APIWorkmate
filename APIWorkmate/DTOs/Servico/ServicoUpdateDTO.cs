namespace APIWorkmate.DTOs.Servico;

public class ServicoUpdateDTO
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public decimal? Preco { get; set; }
    public Guid? SubcategoriaId { get; set; }
    public Guid? PrestadorId { get; set; }
}

