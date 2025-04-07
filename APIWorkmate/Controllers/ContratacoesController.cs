using APIWorkmate.Context;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkmate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContratacoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContratacoesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contratacao>>> GetContratacoes()
    {
        try
        {
            return await _context.Contratacoes
                .Include(c => c.Servico)
                .Include(c => c.Cliente)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Contratacao>> GetContratacao(int id)
    {
        try
        {
            var contratacao = await _context.Contratacoes
                .Include(c => c.Servico)
                .Include(c => c.Cliente)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return contratacao == null ? NotFound("Contratação não encontrada.") : Ok(contratacao);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Contratacao>> PostContratacao(Contratacao contratacao)
    {
        try
        {
            _context.Contratacoes.Add(contratacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContratacao), new { id = contratacao.Id }, contratacao);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> PutContratacao(int id, Contratacao contratacao)
    {
        if (id != contratacao.Id)
            return BadRequest("ID informado não corresponde à contratação.");

        _context.Entry(contratacao).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ContratacaoExiste(id))
                return NotFound("Contratação não encontrada.");
            else
                return StatusCode(500, "Erro ao atualizar contratação.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> DeleteContratacao(int id)
    {
        try
        {
            var contratacao = await _context.Contratacoes.FindAsync(id);
            if (contratacao == null)
                return NotFound("Contratação não encontrada.");

            _context.Contratacoes.Remove(contratacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    private bool ContratacaoExiste(int id)
    {
        return _context.Contratacoes.Any(c => c.Id == id);
    }
}
