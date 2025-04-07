using APIWorkmate.Context;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkmate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        try
        {
            return await _context.Usuarios.AsNoTracking().ToListAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        try
        {
            var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado");
            }
            return usuario;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
    {
        try
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
            {
                return Conflict("Já existe um usuário com este e-mail.");
            }
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
    {

        if (id != usuario.Id)
        {
            return BadRequest("ID informado não corresponde ao ID do usuário.");
        }

        _context.Entry(usuario).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExiste(id))
            {
                return NotFound("Usuário não encontrado.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpGet("{id:int:min(1)}/servicos")]
    public async Task<ActionResult<IEnumerable<Servico>>> GetServicosPorUsuario(int id)
    {
        try
        {
            var usuario = await _context.Usuarios
            .Include(u => u.Servicos)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(usuario.Servicos);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }


    private bool UsuarioExiste(int id)
    {
        return _context.Usuarios.Any(u => u.Id == id);
    }
}
