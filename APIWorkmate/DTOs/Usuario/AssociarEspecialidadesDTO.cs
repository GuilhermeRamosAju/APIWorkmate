namespace APIWorkmate.DTOs.Usuario;

public class AssociarEspecialidadesDTO
{
    public int UsuarioId { get; set; }
    public List<int> CategoriasIds { get; set; } = new();
}
