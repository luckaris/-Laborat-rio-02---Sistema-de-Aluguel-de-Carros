using Auth.API.Core.Dtos;
using Auth.API.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _gerenciarUsuario;
    private readonly RoleManager<IdentityRole> _gerenciarPermissoes;
    private readonly IConfiguration _configuracao;

    /// <summary>
    /// Realiza a injeção de dependência das classes que são responsáveis por gerenciar 
    /// usuários e gerenciar permissões e também a interface IConfiguration para obter 
    /// dados do appsettings.json.
    /// </summary>
    /// <param name="gerenciarUsuario">Gerenciar os usuários.</param>
    /// <param name="gerenciarPermissao">Gerenciar as permissões.</param>
    /// <param name="configuracao">Obter dados do appsettings.json.</param>
    public AuthController(
        UserManager<IdentityUser> gerenciarUsuario,
        RoleManager<IdentityRole> gerenciarPermissao,
        IConfiguration configuracao)
    {
        _gerenciarUsuario = gerenciarUsuario;
        _gerenciarPermissoes = gerenciarPermissao;
        _configuracao = configuracao;
    }

    /// <summary>Rota para adicionar Roles no banco de dados.</summary>
    /// <returns>IActionResult</returns>
    [HttpPost("preencher-permissoes")]
    public async Task<IActionResult> PreencherPermissoes()
    {
        string empresa = EPermissoesUsuario.EMPRESA.ToString();
        string banco = EPermissoesUsuario.BANCO.ToString();
        string cliente = EPermissoesUsuario.CLIENTE.ToString();
        object response;

        bool permissaoDeEmpresaExiste = await _gerenciarPermissoes.RoleExistsAsync(empresa);
        bool permissaoDeBancoExiste = await _gerenciarPermissoes.RoleExistsAsync(banco);
        bool permissaoDeClienteExiste = await _gerenciarPermissoes.RoleExistsAsync(cliente);

        if (permissaoDeEmpresaExiste && permissaoDeBancoExiste && permissaoDeClienteExiste)
        {
            response = new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "As permissões já foram configuradas."
            };
            return BadRequest(response);
        }

        if (!permissaoDeEmpresaExiste) await _gerenciarPermissoes.CreateAsync(new IdentityRole(empresa));
        if (!permissaoDeBancoExiste) await _gerenciarPermissoes.CreateAsync(new IdentityRole(banco));
        if (!permissaoDeClienteExiste) await _gerenciarPermissoes.CreateAsync(new IdentityRole(cliente));

        response = new
        {
            status = StatusCodes.Status200OK,
            message = "Permissões configuradas com sucesso."
        };
        return Ok(response);
    }

    /// <summary>Rota para registrar usuários no banco de dados com a Role padrão "USER".</summary>
    /// <param name="dto">Dados do usuário passado no corpo da requisição.</param>
    /// <returns>IActionResult</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Registrar([FromBody] RegistroDto dto)
    {
        var isExistsUser = await _gerenciarUsuario.FindByNameAsync(dto.NomeDeUsuario);
        if (isExistsUser != null) return BadRequest("O usuário já existe.");
        object response;

        IdentityUser user = new()
        {
            Email = dto.Email,
            UserName = dto.NomeDeUsuario,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var createUserResult = await _gerenciarUsuario.CreateAsync(user, dto.Senha!);
        List<string> errors = new();
        if (!createUserResult.Succeeded)
        {
            foreach (var error in createUserResult.Errors)
            {
                errors.Add(error.Description);
            }
            response = new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = errors
            };
            return BadRequest(response);
        }
        await _gerenciarUsuario.AddToRoleAsync(user, EPermissoesUsuario.CLIENTE.ToString());
        response = new
        {
            status = StatusCodes.Status200OK.ToString(),
            message = "Usuário criado com sucesso."
        };
        return Ok(response);
    }

    /// <summary>Rota para logar o usuário na aplicação.</summary>
    /// <param name="dto">Dados do usuário passado no corpo da requisição.</param>
    /// <returns>IActionResult</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        object response;
        var user = await _gerenciarUsuario.FindByNameAsync(dto.NomeDeUsuario!);
        if (user is null)
        {
            response = new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Usuário não foi encontrado."
            };
            return NotFound(response);
        }

        var senhaEstaCerta = await _gerenciarUsuario.CheckPasswordAsync(user, dto.Senha!);
        if (!senhaEstaCerta)
        {
            response = new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Credenciais inválidas"
            };
            return BadRequest(response);
        }

        var permissoesDoUsuario = await _gerenciarUsuario.GetRolesAsync(user);
        var claimsDeAutenticacao = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("JWTID", Guid.NewGuid().ToString())
        };

        foreach (var permissaoDoUsuario in permissoesDoUsuario)
        {
            claimsDeAutenticacao.Add(new Claim(ClaimTypes.Role, permissaoDoUsuario));
        }
        var token = GerarNovoJsonWebToken(claimsDeAutenticacao);

        response = new
        {
            token,
        };
        return Ok(response);
    }

    /// <summary>Atualiza as permissões de um usuário em específico para BANCO.</summary>
    /// <param name="dto">Dados enviados no corpo da requisição para atualizar as permissões.</param>
    /// <returns>IActionResult</returns>
    [HttpPost("atualizar-permissao-para-banco")]
    public async Task<IActionResult> AtualizarPermissaoParaBanco([FromBody] AtualizarPermissoesDto dto)
    {
        var usuario = await _gerenciarUsuario.FindByNameAsync(dto.NomeDeUsuario);
        object response;

        if(usuario is null)
        {
            response = new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Usuário não foi encontrado."
            };
            return NotFound(response);
        }

        await _gerenciarUsuario.AddToRoleAsync(usuario, EPermissoesUsuario.BANCO.ToString());
        response = new
        {
            status = StatusCodes.Status200OK.ToString(),
            message = $"O usuário com nome de usuário '{dto.NomeDeUsuario}' tem permissões de BANCO."
        };
        return Ok(response);
    }

    /// <summary>Atualiza as permissões de um usuário em específico para EMPRESA.</summary>
    /// <param name="dto">Dados enviados no corpo da requisição para atualizar as permissões.</param>
    /// <returns>IActionResult</returns>
    [HttpPost("atualizar-permissao-para-empresa")]
    public async Task<IActionResult> AtualizarPermissaoParaEmpresa([FromBody] AtualizarPermissoesDto dto)
    {
        var usuario = await _gerenciarUsuario.FindByNameAsync(dto.NomeDeUsuario);
        object response;

        if (usuario is null)
        {
            response = new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Usuário não foi encontrado."
            };
            return NotFound(response);
        }

        await _gerenciarUsuario.AddToRoleAsync(usuario, EPermissoesUsuario.EMPRESA.ToString());
        response = new
        {
            status = StatusCodes.Status200OK.ToString(),
            message = $"O usuário com nome de usuário '{dto.NomeDeUsuario}' tem permissões de EMPRESA."
        };
        return Ok(response);
    }

    /// <summary>Gera um token com base nas claims passadas por parâmetro.</summary>
    /// <param name="claims">Representa as claims passadas por parâmetro.</param>
    /// <returns>string</returns>
    private string GerarNovoJsonWebToken(List<Claim> claims)
    {
        string validIssuer = _configuracao["Jwt:ValidIssuer"]!;
        string validAudience = _configuracao["Jwt:ValidAudience"]!;
        string secret = _configuracao["Jwt:Secret"]!;
        byte[] key = Encoding.UTF8.GetBytes(secret);

        var chaveSecreta = new SymmetricSecurityKey(key);
        var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

        var objetoDeToken = new JwtSecurityToken(
            issuer: validIssuer,
            audience: validAudience,
            expires: DateTime.Now.AddHours(1),
            claims: claims,
            signingCredentials: credenciais);
        string token = new JwtSecurityTokenHandler().WriteToken(objetoDeToken);
        return token;
    }
}