using APIWorkmate.Context;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Http;
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
        return await _context.Servicos.Include(s => s.Prestador).Include(s => s.Categoria).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Servico>> GetServico(int id)
    {
        var servico = await _context.Servicos
            .Include(s => s.Prestador)
            .Include(s => s.Categoria)
            .FirstOrDefaultAsync(s => s.Id == id);

        return servico == null ? NotFound() : Ok(servico);
    }

    [HttpPost]
    public async Task<ActionResult<Servico>> PostServico(Servico servico)
    {
        _context.Servicos.Add(servico);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetServico), new { id = servico.Id }, servico);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutServico(int id, Servico servico)
    {
        if (id != servico.Id) return BadRequest();

        _context.Entry(servico).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServico(int id)
    {
        var servico = await _context.Servicos.FindAsync(id);
        if (servico == null) return NotFound();

        _context.Servicos.Remove(servico);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
