using APIWorkmate.Context;
using APIWorkmate.DTOs.Categoria;
using APIWorkmate.DTOs.Servico;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkmate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            try
            {
                return await _context.Categorias.Include(a => a.Servicos).ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Categoria>> GetCategoria(Guid id)
        {
            try
            {
                var avaliacao = await _context.Categorias
                    .Include(a => a.Servicos)
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
        public async Task<ActionResult<CategoriaReadDTO>> PostCategoria(CategoriaCreateDTO categoriaDto)
        {
            try
            {
                var nomeNormalizado = categoriaDto.Nome.Trim().ToLower();

                var categoriaExistente = await _context.Categorias
                    .AnyAsync(c => c.Nome.Trim().ToLower() == nomeNormalizado);

                if (categoriaExistente)
                    return Conflict("Já existe uma categoria com esse nome.");

                var categoria = new Categoria
                {
                    Nome = categoriaDto.Nome.Trim()
                };

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                var readDto = new CategoriaReadDTO
                {
                    Id = categoria.Id,
                    Nome = categoria.Nome
                };

                return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, readDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao processar requisição.");
            }
        }

        [HttpGet("servicos")]
        public async Task<ActionResult<IEnumerable<CategoriaComServicosDTO>>> GetCategoriasComServicos()
        {
            try
            {
                var categorias = await _context.Categorias
                    .Include(c => c.Servicos)
                    .Select(c => new CategoriaComServicosDTO
                    {
                        Id = c.Id,
                        Nome = c.Nome,
                        Servicos = c.Servicos!.Select(s => new ServicoReadDTO
                        {
                            Id = s.Id,
                            Titulo = s.Titulo,
                            Descricao = s.Descricao,
                            Preco = s.Preco
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(categorias);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar categorias com serviços.");
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> PutCategoria(Guid id, Categoria categoria)
        {
            try
            {
                if (id != categoria.Id) return BadRequest();

                _context.Entry(categoria).State = EntityState.Modified;
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
                var avaliacao = await _context.Categorias.FindAsync(id);
                if (avaliacao == null) return NotFound();

                _context.Categorias.Remove(avaliacao);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        private bool CategoriaExiste(Guid id)
        {
            return _context.Categorias.Any(s => s.Id == id);
        }
    }
}
