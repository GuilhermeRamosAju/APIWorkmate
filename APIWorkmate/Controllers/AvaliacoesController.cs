using APIWorkmate.Context;
using APIWorkmate.DTOs.Avaliacao;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<AvaliacaoReadDTO>> GetAvaliacao(Guid id)
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
        [Authorize]
        public async Task<ActionResult<AvaliacaoReadDTO>> PostAvaliacao([FromBody] AvaliacaoCreateDTO dto)
        {
            var servico = await _context.Servicos.FindAsync(dto.ServicoId);
            if (servico == null)
                return NotFound($"Serviço com ID {dto.ServicoId} não encontrado.");

            var cliente = await _context.Usuarios.FindAsync(dto.ClienteId);
            if (cliente == null)
                return NotFound($"Cliente com ID {dto.ClienteId} não encontrado.");

            var avaliacao = new Avaliacao
            {
                Nota = dto.Nota,
                Comentario = dto.Comentario,
                ServicoId = dto.ServicoId,
                ClienteId = dto.ClienteId
            };

            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            var readDto = new AvaliacaoReadDTO
            {
                Id = avaliacao.Id,
                Nota = avaliacao.Nota,
                Comentario = avaliacao.Comentario,
                DataAvaliacao = avaliacao.DataAvaliacao,
                NomeCliente = cliente.Nome
            };

            return CreatedAtAction(nameof(GetAvaliacao), new { id = avaliacao.Id }, readDto);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> PutAvaliacao(Guid id, Avaliacao avaliacao)
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

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAvaliacao(Guid id)
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

        private bool AvaliacaoExiste(Guid id)
        {
            return _context.Avaliacoes.Any(a => a.Id == id);
        }
    }
}
