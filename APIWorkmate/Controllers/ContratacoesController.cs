using APIWorkmate.Context;
using APIWorkmate.DTOs;
using APIWorkmate.DTOs.Contratacao;
using APIWorkmate.DTOs.Servico;
using APIWorkmate.DTOs.Usuario;
using APIWorkmate.Models;
using Microsoft.AspNetCore.JsonPatch;
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
    public async Task<ActionResult<IEnumerable<ContratacaoReadDTO>>> GetContratacoes()
    {
        try
        {
            var contratacoes = await _context.Contratacoes
                .Include(c => c.Servico)
                .Include(c => c.Cliente)
                .AsNoTracking()
                .ToListAsync();

            var contratacoesDTO = contratacoes.Select(c => new ContratacaoReadDTO
            {
                Id = c.Id,
                DataContratacao = c.DataContratacao,
                Status = c.Status,
                Cliente = new UsuarioReadDTO
                {
                    Id = c.Cliente.Id,
                    Nome = c.Cliente.Nome
                },
                Servico = new ServicoReadDTO
                {
                    Id = c.Servico.Id,
                    Titulo = c.Servico.Titulo
                }
            });

            return Ok(contratacoesDTO);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<ContratacaoReadDTO>> GetContratacao(int id)
    {
        try
        {
            var contratacao = await _context.Contratacoes
                .Include(c => c.Servico)
                .Include(c => c.Cliente)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contratacao == null) return NotFound("Contratação não encontrada.");

            var dto = new ContratacaoReadDTO
            {
                Id = contratacao.Id,
                DataContratacao = contratacao.DataContratacao,
                Status = contratacao.Status,
                Cliente = new UsuarioReadDTO
                {
                    Id = contratacao.Cliente.Id,
                    Nome = contratacao.Cliente.Nome
                },
                Servico = new ServicoReadDTO
                {
                    Id = contratacao.Servico.Id,
                    Titulo = contratacao.Servico.Titulo
                }
            };

            return Ok(dto);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostContratacao(ContratacaoCreateDTO dto)
    {
        try
        {
            var contratacao = new Contratacao
            {
                ClienteId = dto.ClienteId,
                ServicoId = dto.ServicoId,
                DataContratacao = dto.DataContratacao,
                Status = dto.Status
            };

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
    public async Task<IActionResult> PutContratacao(int id, ContratacaoUpdateDTO dto)
    {
        if (id != dto.Id)
            return BadRequest("ID informado não corresponde à contratação.");

        var contratacao = await _context.Contratacoes.FindAsync(id);
        if (contratacao == null)
            return NotFound("Contratação não encontrada.");

        contratacao.ClienteId = dto.ClienteId;
        contratacao.ServicoId = dto.ServicoId;
        contratacao.DataContratacao = dto.DataContratacao;
        contratacao.Status = dto.Status;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500, "Erro ao atualizar contratação.");
        }
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> PatchContratacao(int id, [FromBody] ContratacaoPatchDTO patchDto)
    {
        if (id != patchDto.Id)
            return BadRequest("O ID informado não corresponde à contratação.");

        var contratacao = await _context.Contratacoes.FindAsync(id);
        if (contratacao == null)
            return NotFound("Contratação não encontrada.");

        if (patchDto.ClienteId.HasValue)
            contratacao.ClienteId = patchDto.ClienteId.Value;

        if (patchDto.ServicoId.HasValue)
            contratacao.ServicoId = patchDto.ServicoId.Value;

        if (patchDto.DataContratacao.HasValue)
            contratacao.DataContratacao = patchDto.DataContratacao.Value;

        if (patchDto.Status is not null)
            contratacao.Status = patchDto.Status;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Erro ao aplicar o patch na contratação.");
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
