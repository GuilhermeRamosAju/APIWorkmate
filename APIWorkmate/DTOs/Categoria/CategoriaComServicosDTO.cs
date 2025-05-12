using APIWorkmate.DTOs.Servico;

namespace APIWorkmate.DTOs.Categoria;

public class CategoriaComServicosDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public List<ServicoReadDTO> Servicos { get; set; } = new();
}
