using APIWorkmate.Context;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
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
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            try
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id, categoria });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
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

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeleteAvaliacao(int id)
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

        private bool CategoriaExiste(int id)
        {
            return _context.Categorias.Any(s => s.Id == id);
        }
    }
}
