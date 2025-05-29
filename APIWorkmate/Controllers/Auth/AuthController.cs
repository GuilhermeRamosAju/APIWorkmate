using APIWorkmate.DTOs;
using APIWorkmate.Enums;
using APIWorkmate.Interfaces;
using APIWorkmate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIWorkmate.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ITokenService tokenService, UserManager<Usuario> userManager, IConfiguration configuration) : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly UserManager<Usuario> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                out int refreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;

            user.RefreshTokenExpireTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email!);

        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Erro", Message = "Usuário já existe!" });
        }

        Usuario user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName,
            Tipo = model.TipoUsuario,
            Nome = model.Nome,
            FotoPerfil = model.FotoPerfil,
            Cidade = model.Cidade,
            Estado = model.Estado,
            Disponibilidade = model.Disponibilidade,
            Formacao = model.Formacao,
            Experiencia = model.Experiencia
        };

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response
                {
                    Status = "Erro",
                    Message = $"Falha na criação de usuário. {string.Join(", ", result.Errors.Select(e => e.Description))}"
                });
        }
        return Ok(new Response { Status = "Sucesso", Message = "Usuário criado com sucesso!" });
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Requisição inválida!");
        }

        string? accessToken = tokenModel.AcessToken ?? throw new ArgumentException(nameof(tokenModel));
        string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentException(nameof(tokenModel));

        var principal = _tokenService.GetClaimsPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal == null)
        {
            return BadRequest("Token/Refresh token inválido!");
        }

        string username = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken
                         || user.RefreshTokenExpireTime <= DateTime.Now)
        {
            return BadRequest("Token/Refresh token inválido!");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken,
        });
    }
}
