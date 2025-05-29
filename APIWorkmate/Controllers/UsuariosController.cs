using APIWorkmate.Context;
using APIWorkmate.DTOs.Categoria;
using APIWorkmate.DTOs.Usuario;
using APIWorkmate.Enums;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkmate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        try
        {
            return await _context.Usuarios.AsNoTracking().ToListAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpGet("prestadores")]
    public async Task<ActionResult<IEnumerable<UsuarioReadDTO>>> GetUsuariosPrestadores()
    {
        try
        {
            var prestadores = await _context.Usuarios
                .Where(u => u.Tipo == TipoUsuario.Prestador)
                .Select(usuario => new UsuarioReadDTO
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Telefone = usuario.Telefone,
                    Tipo = usuario.Tipo,
                    FotoPerfil = usuario.FotoPerfil,
                    Cidade = usuario.Cidade,
                    Estado = usuario.Estado,
                    Disponibilidade = usuario.Disponibilidade,
                    Formacao = usuario.Formacao,
                    Experiencia = usuario.Experiencia
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(prestadores);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpGet("{id:Guid}/avaliacoes")]
    public async Task<ActionResult<object>> GetAvaliacoesDoUsuario(Guid id)
    {
        try
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Avaliacoes)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var avaliacoes = usuario.Avaliacoes?.ToList() ?? new List<Avaliacao>();

            double mediaNotas = avaliacoes.Any()
                ? Math.Round(avaliacoes.Average(a => a.Nota), 2)
                : 0.0;

            return Ok(new
            {
                UsuarioId = id,
                MediaNota = mediaNotas,
                Avaliacoes = avaliacoes.Select(a => new
                {
                    a.Id,
                    a.Nota,
                    a.Comentario,
                    a.DataAvaliacao,
                    a.ServicoId
                })
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UsuarioReadDTO>> GetUsuario(Guid id)
    {
        try
        {
            var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado");
            }

            var usuarioRead = new UsuarioReadDTO
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Tipo = usuario.Tipo,
                FotoPerfil = usuario.FotoPerfil,
                Cidade = usuario.Cidade,
                Estado = usuario.Estado,
                Disponibilidade = usuario.Disponibilidade,
                Formacao = usuario.Formacao,
                Experiencia = usuario.Experiencia,
            };

            return usuarioRead;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpGet("filtrar")]
    public async Task<ActionResult> FiltrarUsuarios(
       [FromQuery] string? nome,
       [FromQuery] string? localizacao,
       [FromQuery] string? subcategoriaNome,
       [FromQuery] double? notaMinima
   )
    {
        try
        {
            var query = _context.Usuarios!
                .Include(u => u.Servicos!)
                    .ThenInclude(s => s.Subcategoria)
                .Include(u => u.Servicos!)
                    .ThenInclude(s => s.Avaliacoes)
                .Include(u => u.Especialidades)
                .Where(u => u.Tipo == TipoUsuario.Prestador)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(u => u.Nome.Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(localizacao))
            {
                query = query.Where(u => u.Estado.Contains(localizacao) || u.Cidade.Contains(localizacao));
            }

            if (!string.IsNullOrWhiteSpace(subcategoriaNome))
            {
                query = query.Where(u => u.Especialidades!.Any(sc => sc.Nome.StartsWith(subcategoriaNome)));
            }

            var usuariosFiltrados = await query
                .AsNoTracking()
                .Select(u => new
                {
                    u.Id,
                    u.Nome,
                    u.Email,
                    u.FotoPerfil,
                    u.Estado,
                    u.Cidade,
                    u.Disponibilidade,
                    u.Formacao,
                    u.Experiencia,
                    Especialidades = u.Especialidades!.Select(sc => new
                    {
                        sc.Id,
                        sc.Nome,
                        Categoria = sc.Categoria != null ? new
                        {
                            sc.Categoria.Id,
                            sc.Categoria.Nome
                        } : null
                    }),
                    Servicos = u.Servicos!.Select(s => new
                    {
                        s.Id,
                        s.Titulo,
                        Subcategoria = s.Subcategoria != null ? s.Subcategoria.Nome : null,
                        Avaliacoes = s.Avaliacoes!.Select(a => new
                        {
                            a.Id,
                            a.Nota,
                            a.Comentario
                        })
                    }),
                    MediaNota = u.Servicos!
                        .Where(s => s.Avaliacoes != null && s.Avaliacoes.Any())
                        .SelectMany(s => s.Avaliacoes!)
                        .Any()
                            ? Math.Round(u.Servicos!
                                .Where(s => s.Avaliacoes != null)
                                .SelectMany(s => s.Avaliacoes!)
                                .Average(a => a.Nota), 2)
                            : 0.0
                })
                .ToListAsync();

            if (notaMinima.HasValue)
            {
                usuariosFiltrados = usuariosFiltrados
                    .Where(u => u.MediaNota >= notaMinima.Value)
                    .ToList();
            }

            return Ok(usuariosFiltrados);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar a requisição.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioCreateDTO>> CreateUsuario([FromBody] UsuarioCreateDTO usuarioDto)
    {
        try
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuarioDto.Email))
            {
                return Conflict("Já existe um usuário com este e-mail.");
            }

            if (!Enum.IsDefined(typeof(TipoUsuario), usuarioDto.Tipo))
            {
                return BadRequest("Tipo de usuário inválido.");
            }

            var novoUsuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                Telefone = usuarioDto.Telefone,
                Tipo = usuarioDto.Tipo,
                FotoPerfil = usuarioDto.FotoPerfil,
                Cidade = usuarioDto.Cidade,
                Estado = usuarioDto.Estado,
                Disponibilidade = usuarioDto.Disponibilidade,
                Formacao = usuarioDto.Formacao,
                Experiencia = usuarioDto.Experiencia,
                DataCadastro = DateTime.UtcNow
            };

            var hasher = new PasswordHasher<Usuario>();
            novoUsuario.SenhaHash = hasher.HashPassword(novoUsuario, usuarioDto.SenhaHash);

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            var readNovoUsuario = new UsuarioReadDTO
            {
                Nome = novoUsuario.Nome,
                Email = novoUsuario.Email,
                Telefone = novoUsuario.Telefone,
                Tipo = novoUsuario.Tipo,
                FotoPerfil = novoUsuario.FotoPerfil,
                Cidade = novoUsuario.Cidade,
                Estado = novoUsuario.Estado,
                Disponibilidade = novoUsuario.Disponibilidade,
                Formacao = novoUsuario.Formacao,
                Experiencia = novoUsuario.Experiencia,
            };

            return CreatedAtAction(nameof(GetUsuario), new { id = novoUsuario.Id }, readNovoUsuario);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpGet("{id}/especialidades")]
    public async Task<ActionResult<IEnumerable<CategoriaReadDTO>>> GetEspecialidades(Guid id)
    {
        try
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Especialidades)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null) return NotFound("Usuário não encontrado.");

            var especialidades = usuario.Especialidades?
                .Select(c => new CategoriaReadDTO
                {
                    Id = c.Id,
                    Nome = c.Nome
                })
                .ToList();

            return Ok(especialidades);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar requisição.");
        }
    }
    [HttpPost("associar-especialidades")]
    public async Task<IActionResult> AssociarEspecialidades([FromBody] AssociarEspecialidadesDTO dto)
    {
        try
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Especialidades)
                .FirstOrDefaultAsync(u => u.Id == dto.UsuarioId);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            var subcategorias = await _context.Subcategorias
                .Where(sc => dto.SubcategoriasIds.Contains(sc.Id))
                .ToListAsync();

            if (!subcategorias.Any())
                return BadRequest("Nenhuma subcategoria válida fornecida.");

            var idsJaAssociados = usuario.Especialidades.Select(e => e.Id).ToHashSet();
            var subcategoriasAdicionadas = new List<string>();
            var subcategoriasIgnoradas = new List<string>();

            foreach (var subcategoria in subcategorias)
            {
                if (!idsJaAssociados.Contains(subcategoria.Id))
                {
                    usuario.Especialidades.Add(subcategoria);
                    subcategoriasAdicionadas.Add(subcategoria.Nome);
                }
                else
                {
                    subcategoriasIgnoradas.Add(subcategoria.Nome);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Processo concluído.",
                adicionadas = subcategoriasAdicionadas,
                jaAssociadas = subcategoriasIgnoradas
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao associar especialidades.");
        }
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateUsuario(Guid id, Usuario usuario)
    {

        if (id != usuario.Id)
        {
            return BadRequest("ID informado não corresponde ao ID do usuário.");
        }

        _context.Entry(usuario).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExiste(id))
            {
                return NotFound("Usuário não encontrado.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
            }
        }

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteUsuario(Guid id)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpPatch("{id:Guid}")]
    public async Task<IActionResult> PatchUsuario(Guid id, [FromBody] UsuarioUpdateDTO usuarioDto)
    {
        try
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            if (!string.IsNullOrWhiteSpace(usuarioDto.Nome))
                usuarioExistente.Nome = usuarioDto.Nome;

            if (!string.IsNullOrWhiteSpace(usuarioDto.Email))
                usuarioExistente.Email = usuarioDto.Email;

            if (!string.IsNullOrWhiteSpace(usuarioDto.Telefone))
                usuarioExistente.Telefone = usuarioDto.Telefone;

            if (usuarioDto.Tipo.HasValue)
                usuarioExistente.Tipo = usuarioDto.Tipo.Value;

            if (!string.IsNullOrWhiteSpace(usuarioDto.FotoPerfil))
                usuarioExistente.FotoPerfil = usuarioDto.FotoPerfil;

            if (!string.IsNullOrWhiteSpace(usuarioDto.Cidade))
                usuarioExistente.Cidade = usuarioDto.Cidade;

            if (!string.IsNullOrWhiteSpace(usuarioDto.Estado))
                usuarioExistente.Estado = usuarioDto.Estado;

            if (!string.IsNullOrWhiteSpace(usuarioDto.Disponibilidade))
                usuarioExistente.Disponibilidade = usuarioDto.Disponibilidade;

            if (!string.IsNullOrWhiteSpace(usuarioDto.Formacao))
                usuarioExistente.Formacao = usuarioDto.Formacao;

            if (!string.IsNullOrWhiteSpace(usuarioDto.Experiencia))
                usuarioExistente.Experiencia = usuarioDto.Experiencia;

            if (!string.IsNullOrWhiteSpace(usuarioDto.Senha))
            {
                var hasher = new PasswordHasher<Usuario>();
                usuarioExistente.SenhaHash = hasher.HashPassword(usuarioExistente, usuarioDto.Senha);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    [HttpGet("{id:Guid}/servicos")]
    public async Task<ActionResult<IEnumerable<Servico>>> GetServicosPorUsuario(Guid id)
    {
        try
        {
            var usuario = await _context.Usuarios
            .Include(u => u.Servicos)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(usuario.Servicos);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua requisição.");
        }
    }

    private bool UsuarioExiste(Guid id)
    {
        return _context.Usuarios.Any(u => u.Id == id);
    }
}
