using APIWorkmate.Context;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkmate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MensagensController : ControllerBase
{
    private readonly AppDbContext _context;

    public MensagensController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mensagem>>> GetMensagens()
    {
        try
        {
            return await _context.Mensagens
                .Include(m => m.Remetente)
                .Include(m => m.Destinatario)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Mensagem>> GetMensagem(int id)
    {
        try
        {
            var mensagem = await _context.Mensagens
                .Include(m => m.Remetente)
                .Include(m => m.Destinatario)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return mensagem == null ? NotFound("Mensagem não encontrada.") : Ok(mensagem);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Mensagem>> PostMensagem(Mensagem mensagem)
    {
        try
        {
            _context.Mensagens.Add(mensagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMensagem), new { id = mensagem.Id }, mensagem);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> PutMensagem(int id, Mensagem mensagem)
    {
        if (id != mensagem.Id)
            return BadRequest("ID informado não corresponde à mensagem.");

        _context.Entry(mensagem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MensagemExiste(id))
                return NotFound("Mensagem não encontrada.");
            else
                return StatusCode(500, "Erro ao atualizar mensagem.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> DeleteMensagem(int id)
    {
        try
        {
            var mensagem = await _context.Mensagens.FindAsync(id);
            if (mensagem == null)
                return NotFound("Mensagem não encontrada.");

            _context.Mensagens.Remove(mensagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    private bool MensagemExiste(int id)
    {
        return _context.Mensagens.Any(m => m.Id == id);
    }
}

