using APIWorkmate.Context;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkmate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServicosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ServicosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Servico>>> GetServicos()
    {
        try
        {
            return await _context.Servicos.Include(s => s.Prestador).Include(s => s.Categoria).AsNoTracking().ToListAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Servico>> GetServico(int id)
    {
        try
        {
            var servico = await _context.Servicos
            .Include(s => s.Prestador)
            .Include(s => s.Categoria)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

            return servico == null ? NotFound() : Ok(servico);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Servico>> PostServico(Servico servico)
    {
        try
        {
            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServico), new { id = servico.Id }, servico);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> UpdateServico(int id, Servico servico)
    {
        if (id != servico.Id)
            return BadRequest("ID informado não corresponde ao serviço.");

        _context.Entry(servico).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServicoExiste(id))
                return NotFound("Serviço não encontrado.");
            else
                return StatusCode(500, "Erro ao atualizar serviço.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> DeleteServico(int id)
    {
        try
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null) return NotFound();

            _context.Servicos.Remove(servico);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    private bool ServicoExiste(int id)
    {
        return _context.Servicos.Any(s => s.Id == id);
    }
}
