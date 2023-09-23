using Autenticacao.API.Repository;
using Autenticacao.API.Service;
using Microsoft.AspNetCore.Mvc;
using Models.AutenticacaoModels.Dtos;

namespace Autenticacao.API.Controllers;

[Route("api/autenticacao")]
[ApiController]
public class AutenticacaoController : ControllerBase
{
    private readonly IConfiguration _configuracao;
    private readonly IAutenticacaoRepositorio _autenticacaoRepositorio;
    /// <summary>
    /// O construtor dessa classe realiza a Injeção de Dependência da Interface responsável
    /// por acessar no Banco de Dados, ao realizar essa Injeção de Dependência, esse usa os
    /// métodos implementados na classe que implementa IAutenticacaoRepositorio, definimos
    /// isso na classe Program por meio do método AddScoped. Estamos também fazendo a injeção
    /// de dependência a Interface de configuração para obtermos os valores de algumas chaves
    /// JSON do appsettings.
    /// </summary>
    /// <param name="autenticacaoRepositorio">
    /// Referência a Interface no qual estamos realizando Injeção de Dependência.
    /// </param>
    public AutenticacaoController(IAutenticacaoRepositorio autenticacaoRepositorio, IConfiguration configuracao)
    {
        _autenticacaoRepositorio = autenticacaoRepositorio;
        _configuracao = configuracao;
    }

    /// <summary>
    /// Cadastra um usuário no banco de dados.
    /// </summary>
    /// <param name="dto">Dados informados pelo usuário no corpo da requisição.</param>
    /// <returns>IActionResult</returns>
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] CadastrarUsuarioDto dto)
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
        string token = TokenService.GerarNovoJwt(resposta, _configuracao["Jwt:ChaveSecreta"]!);
        return Created(
            $"https://localhost:7190/api/cliente/{resposta.Id}",
            new
            {
                token
            });
    }

    /// <summary>
    /// Realizar o login de um usuário na API.
    /// </summary>
    /// <param name="dto">Credenciais de acesso informadas pelo usuário no banco de dados.</param>
    /// <returns>IActionResult</returns>
    [HttpPost("logar")]
    public async Task<IActionResult> Logar([FromBody] LogarUsuarioDto dto)
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
        string token = TokenService.GerarNovoJwt(resposta, _configuracao["Jwt:ChaveSecreta"]!);
        return Ok(new
        {
            token
        });
    }
}
