using Autenticacao.API.Dto;
using Autenticacao.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Autenticacao.API.Controllers;

[Route("api/autenticacao")]
[ApiController]
public class AutenticacaoController : ControllerBase
{
    private readonly IAutenticacaoRepositorio _autenticacaoRepositorio;
    public AutenticacaoController(IAutenticacaoRepositorio autenticacaoRepositorio)
    {
        _autenticacaoRepositorio = autenticacaoRepositorio;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] CadastrarDto dto)
    {
        var resposta = await _autenticacaoRepositorio.Criar(dto);
        if (resposta is null)
        {
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Houve algum erro durante o cadastro do usuário."
            });
        }
        return Created(
            $"https://localhost:7190/api/cliente/{resposta.Id}",
            new
            {
                status = StatusCodes.Status201Created.ToString(),
                message = "Usuário cadastrado com sucesso."
            });
    }

    [HttpPost("logar")]
    public async Task<IActionResult> Logar([FromBody] LogarDto dto)
    {
        var resposta = await _autenticacaoRepositorio.BuscarPelasCredenciais(dto);
        if (resposta is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Usuário não foi encontrado."
            });
        }
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, resposta.Nome),
            new Claim(ClaimTypes.Role, resposta.Permissao)
        };

        var token = _autenticacaoRepositorio.GerarNovoJwt(claims);
        return Ok(new
        {
            token
        });
    }
}
