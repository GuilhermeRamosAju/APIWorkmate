using APIWorkmate.Context;
using APIWorkmate.DTOs.Servico;
using APIWorkmate.Enums;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkmate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServicosController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServicoReadDTO>>> GetServicos()
    {
        try
        {
            var servicos = await _context.Servicos
                .Include(s => s.Prestador)
                .Include(s => s.Subcategoria)
                .AsNoTracking()
                .ToListAsync();

            var servicoDTOs = servicos.Select(s => new ServicoReadDTO
            {
                Id = s.Id,
                Titulo = s.Titulo,
                Descricao = s.Descricao,
                Preco = s.Preco,
                SubcategoriaNome = s.Subcategoria.Nome,
                PrestadorNome = s.Prestador.Nome
            });

            return Ok(servicoDTOs);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar os serviços.");
        }
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<ServicoReadDTO>> GetServico(Guid id)
    {
        try
        {
            var servico = await _context.Servicos
                .Include(s => s.Prestador)
                .Include(s => s.Subcategoria)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (servico == null) return NotFound();

            var servicoDTO = new ServicoReadDTO
            {
                Id = servico.Id,
                Titulo = servico.Titulo,
                Descricao = servico.Descricao,
                Preco = servico.Preco,
                SubcategoriaNome = servico.Subcategoria.Nome,
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
            var usuario = await _context.Usuarios
                .Include(u => u.Especialidades)
                .FirstOrDefaultAsync(u => u.Id == servicoDTO.PrestadorId);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            if (usuario.Tipo != TipoUsuario.Prestador)
                return BadRequest("Apenas usuários do tipo Prestador podem criar serviços.");

            var subcategoria = await _context.Subcategorias.FindAsync(servicoDTO.SubcategoriaId);
            if (subcategoria == null)
                return BadRequest("Subcategoria inválida.");

            var jaAssociado = usuario.Especialidades.Any(e => e.Id == subcategoria.Id);

            if (!jaAssociado)
            {
                usuario.Especialidades.Add(subcategoria);
            }

            var servico = new Servico
            {
                Titulo = servicoDTO.Titulo,
                Descricao = servicoDTO.Descricao,
                Preco = servicoDTO.Preco,
                SubcategoriaId = servicoDTO.SubcategoriaId,
                PrestadorId = servicoDTO.PrestadorId
            };

            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            var servicoCriado = await _context.Servicos
                .Include(s => s.Prestador)
                .Include(s => s.Subcategoria)
                .FirstOrDefaultAsync(s => s.Id == servico.Id);

            var servicoReadDTO = new ServicoReadDTO
            {
                Id = servicoCriado!.Id,
                Titulo = servicoCriado.Titulo,
                Descricao = servicoCriado.Descricao,
                Preco = servicoCriado.Preco,
                SubcategoriaNome = servicoCriado.Subcategoria.Nome,
                PrestadorNome = servicoCriado.Prestador.Nome
            };

            return CreatedAtAction(nameof(GetServico), new { id = servico.Id }, servicoReadDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao criar o serviço: {ex.Message}");
        }
    }


    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateServico(Guid id, ServicoUpdateDTO servicoDTO)
    {
        if (id != servicoDTO.Id)
            return BadRequest("ID informado não corresponde ao serviço.");

        var servicoExistente = await _context.Servicos.FindAsync(id);
        if (servicoExistente == null) return NotFound("Serviço não encontrado.");

        if (servicoDTO.Titulo is null ||
            servicoDTO.Descricao is null ||
            !servicoDTO.Preco.HasValue ||
            !servicoDTO.SubcategoriaId.HasValue ||
            !servicoDTO.PrestadorId.HasValue)
        {
            return BadRequest("Todos os campos são obrigatórios para atualização completa (PUT).");
        }

        servicoExistente.Titulo = servicoDTO.Titulo;
        servicoExistente.Descricao = servicoDTO.Descricao;
        servicoExistente.Preco = servicoDTO.Preco.Value;
        servicoExistente.SubcategoriaId = servicoDTO.SubcategoriaId.Value;
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

    [HttpPatch("{id:Guid}")]
    public async Task<IActionResult> PatchServico(Guid id, [FromBody] ServicoUpdateDTO patchDto)
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

        if (patchDto.SubcategoriaId.HasValue)
            servico.SubcategoriaId = patchDto.SubcategoriaId.Value;

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


    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteServico(Guid id)
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
