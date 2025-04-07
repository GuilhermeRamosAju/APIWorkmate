using APIWorkmate.Context;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkmate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvaliacoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> GetAvaliacoes()
        {
            try
            {
                return await _context.Avaliacoes.Include(a => a.Cliente).Include(a => a.Servico).ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Avaliacao>> GetAvaliacao(int id)
        {
            try
            {
                var avaliacao = await _context.Avaliacoes
                    .Include(a => a.Cliente)
                    .Include(a => a.Servico)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);

                return avaliacao == null ? NotFound() : Ok(avaliacao);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Avaliacao>> PostAvaliacao(Avaliacao avaliacao)
        {
            try
            {
                _context.Avaliacoes.Add(avaliacao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAvaliacao), new { id = avaliacao.Id, avaliacao });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutAvaliacao(int id, Avaliacao avaliacao)
        {
            try
            {
                if (id != avaliacao.Id) return BadRequest();

                _context.Entry(avaliacao).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeleteAvaliacao(int id)
        {
            try
            {
                var avaliacao = await _context.Avaliacoes.FindAsync(id);
                if (avaliacao == null) return NotFound();

                _context.Avaliacoes.Remove(avaliacao);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        private bool AvaliacaoExiste(int id)
        {
            return _context.Avaliacoes.Any(a => a.Id == id);
        }
    }
}
