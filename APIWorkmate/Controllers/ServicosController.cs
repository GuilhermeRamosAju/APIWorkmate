using APIWorkmate.Context;
using APIWorkmate.DTOs.Servico;
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
    public async Task<ActionResult<IEnumerable<ServicoReadDTO>>> GetServicos()
    {
        try
        {
            var servicos = await _context.Servicos
                .Include(s => s.Prestador)
                .Include(s => s.Categoria)
                .AsNoTracking()
                .ToListAsync();

            var servicoDTOs = servicos.Select(s => new ServicoReadDTO
            {
                Id = s.Id,
                Titulo = s.Titulo,
                Descricao = s.Descricao,
                Preco = s.Preco,
                CategoriaNome = s.Categoria.Nome,
                PrestadorNome = s.Prestador.Nome
            });

            return Ok(servicoDTOs);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar os serviços.");
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<ServicoReadDTO>> GetServico(int id)
    {
        try
        {
            var servico = await _context.Servicos
                .Include(s => s.Prestador)
                .Include(s => s.Categoria)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (servico == null) return NotFound();

            var servicoDTO = new ServicoReadDTO
            {
                Id = servico.Id,
                Titulo = servico.Titulo,
                Descricao = servico.Descricao,
                Preco = servico.Preco,
                CategoriaNome = servico.Categoria.Nome,
                PrestadorNome = servico.Prestador.Nome
            };

            return Ok(servicoDTO);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar o serviço.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ServicoReadDTO>> PostServico(ServicoCreateDTO servicoDTO)
    {
        try
        {
            var servico = new Servico
            {
                Titulo = servicoDTO.Titulo,
                Descricao = servicoDTO.Descricao,
                Preco = servicoDTO.Preco,
                CategoriaId = servicoDTO.CategoriaId,
                PrestadorId = servicoDTO.PrestadorId
            };

            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            var servicoCriado = await _context.Servicos
                .Include(s => s.Prestador)
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(s => s.Id == servico.Id);

            var servicoReadDTO = new ServicoReadDTO
            {
                Id = servicoCriado!.Id,
                Titulo = servicoCriado.Titulo,
                Descricao = servicoCriado.Descricao,
                Preco = servicoCriado.Preco,
                CategoriaNome = servicoCriado.Categoria.Nome,
                PrestadorNome = servicoCriado.Prestador.Nome
            };

            return CreatedAtAction(nameof(GetServico), new { id = servico.Id }, servicoReadDTO);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar o serviço.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> UpdateServico(int id, ServicoUpdateDTO servicoDTO)
    {
        if (id != servicoDTO.Id)
            return BadRequest("ID informado não corresponde ao serviço.");

        var servicoExistente = await _context.Servicos.FindAsync(id);
        if (servicoExistente == null) return NotFound("Serviço não encontrado.");

        if (servicoDTO.Titulo is null ||
            servicoDTO.Descricao is null ||
            !servicoDTO.Preco.HasValue ||
            !servicoDTO.CategoriaId.HasValue ||
            !servicoDTO.PrestadorId.HasValue)
        {
            return BadRequest("Todos os campos são obrigatórios para atualização completa (PUT).");
        }

        servicoExistente.Titulo = servicoDTO.Titulo;
        servicoExistente.Descricao = servicoDTO.Descricao;
        servicoExistente.Preco = servicoDTO.Preco.Value;
        servicoExistente.CategoriaId = servicoDTO.CategoriaId.Value;
        servicoExistente.PrestadorId = servicoDTO.PrestadorId.Value;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500, "Erro ao atualizar serviço.");
        }
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> PatchServico(int id, [FromBody] ServicoUpdateDTO patchDto)
    {
        if (id != patchDto.Id)
            return BadRequest("O ID informado não corresponde ao serviço.");

        var servico = await _context.Servicos.FindAsync(id);
        if (servico == null) return NotFound("Serviço não encontrado.");

        if (patchDto.Titulo is not null)
            servico.Titulo = patchDto.Titulo;

        if (patchDto.Descricao is not null)
            servico.Descricao = patchDto.Descricao;

        if (patchDto.Preco.HasValue)
            servico.Preco = patchDto.Preco.Value;

        if (patchDto.CategoriaId.HasValue)
            servico.CategoriaId = patchDto.CategoriaId.Value;

        if (patchDto.PrestadorId.HasValue)
            servico.PrestadorId = patchDto.PrestadorId.Value;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Erro ao aplicar o patch no serviço.");
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar o serviço.");
        }
    }
}
