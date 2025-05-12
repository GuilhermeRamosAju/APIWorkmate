using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIWorkmate.Models;
using APIWorkmate.Context;
using APIWorkmate.DTOs.Subcategoria;

namespace APIWorkmate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubcategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubcategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubcategoriaDTO dto)
    {
        var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
        if (categoria == null)
            return NotFound("Categoria não encontrada.");

        var subcategoria = new Subcategoria
        {
            Nome = dto.Nome,
            CategoriaId = dto.CategoriaId
        };

        _context.Subcategorias.Add(subcategoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = subcategoria.Id }, subcategoria);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadSubcategoriaDTO>>> GetAll()
    {
        var subcategorias = await _context.Subcategorias
            .Include(sc => sc.Categoria)
            .Select(sc => new ReadSubcategoriaDTO
            {
                Id = sc.Id,
                Nome = sc.Nome,
                CategoriaNome = sc.Categoria!.Nome
            })
            .ToListAsync();

        return Ok(subcategorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReadSubcategoriaDTO>> GetById(Guid id)
    {
        var subcategoria = await _context.Subcategorias
            .Include(sc => sc.Categoria)
            .Where(sc => sc.Id == id)
            .Select(sc => new ReadSubcategoriaDTO
            {
                Id = sc.Id,
                Nome = sc.Nome,
                CategoriaNome = sc.Categoria!.Nome
            })
            .FirstOrDefaultAsync();

        if (subcategoria == null)
            return NotFound("Subcategoria não encontrada.");

        return Ok(subcategoria);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var subcategoria = await _context.Subcategorias.FindAsync(id);
        if (subcategoria == null)
            return NotFound("Subcategoria não encontrada.");

        _context.Subcategorias.Remove(subcategoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
