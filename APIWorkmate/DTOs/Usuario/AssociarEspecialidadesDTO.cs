namespace APIWorkmate.DTOs.Usuario;

public class AssociarEspecialidadesDTO
{
    public Guid UsuarioId { get; set; }
    public List<Guid> SubcategoriasIds { get; set; } = [];
}
