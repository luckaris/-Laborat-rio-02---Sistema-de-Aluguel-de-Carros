using Automoveis.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Models.AutomoveisModels.Dto;

namespace Automoveis.API.Controllers;

[Route("api/automoveis")]
[ApiController]
public class AutomovelController : ControllerBase
{
    private readonly IAutomovelRepositorio _automovelRepositorio;
    /// <summary>
    /// O construtor dessa classe realiza a Injeção de Dependência da classe que
    /// implementa a Interface passada por parâmetro.
    /// </summary>
    /// <param name="automovelRepositorio">Interface da classe de Serviço colocada na classe Program.</param>
    public AutomovelController(IAutomovelRepositorio automovelRepositorio)
    {
        _automovelRepositorio = automovelRepositorio;
    }

    /// <summary>
    /// Obtendo todos os automóveis.
    /// </summary>
    /// <param name="paginacao">Quantidade de itens que serão retornados.</param>
    /// <returns>IActionResult</returns>
    [HttpGet("{paginacao:int}")]
    public async Task<IActionResult> ObterTodos([FromRoute] int paginacao)
    {
        var dados = await _automovelRepositorio.ObterTodos(paginacao);
        return Ok(dados);
    }

    /// <summary>
    /// Obtendo um automóvel pela placa.
    /// </summary>
    /// <param name="placa">Placa do automóvel.</param>
    /// <returns>IActionResult</returns>
    [HttpGet("{placa}")]
    public async Task<IActionResult> ObterPelaPlaca([FromRoute] string placa)
    {
        var dado = await _automovelRepositorio.ObterPelaPlaca(placa);
        if (dado is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Não existe nenhum automóvel com essa placa"
            });
        }
        return Ok(dado);
    }

    /// <summary>
    /// Realiza uma pesquisa/filtros sobre automóveis.
    /// </summary>
    /// <param name="dto">Parâmetros do filtro.</param>
    /// <returns>IActionResult</returns>
    [HttpPost("pesquisar")]
    public async Task<IActionResult> Pesquisar([FromBody] PesquisarAutomovelDto dto)
    {
        var dados = await _automovelRepositorio.Pesquisar(dto);
        return Ok(dados);
    }

    /// <summary>
    /// Cadastra um automóvel no banco de dados.
    /// </summary>
    /// <param name="dto">Dados de um automóvel a ser cadastrado.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarAutomovelDto dto)
    {
        var dado = await _automovelRepositorio.Cadastrar(dto);
        if (dado is null)
        {
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Erro ao cadastrar no banco de dados",
            });
        }
        return Created($"https://localhost:7060/automoveis/{dado.Placa}",
            new
            {
                status = StatusCodes.Status201Created.ToString(),
                message = "Cadastrado com sucesso"
            });
    }

    /// <summary>
    /// Atualiza os dados de um automóvel.
    /// </summary>
    /// <param name="placa">Placa do automóvel que terá seus dados atualizados.</param>
    /// <param name="dto">Novos dados do automóvel que terá os dados atualizados.</param>
    /// <returns>IActionResult</returns>
    [HttpPut("{placa}")]
    public async Task<IActionResult> Atualizar([FromRoute] string placa, [FromBody] AtualizarAutomovelDto dto)
    {
        var dado = await _automovelRepositorio.Atualizar(placa, dto);
        if (dado is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Erro ao atualizar",
            });
        }
        return NoContent();
    }

    /// <summary>
    /// Apagar um automóvel do banco de dados.
    /// </summary>
    /// <param name="placa">Placa do automóvel.</param>
    /// <returns>IActionResult</returns>
    [HttpDelete("{placa}")]
    public async Task<IActionResult> Apagar([FromRoute] string placa)
    {
        var dado = await _automovelRepositorio.Apagar(placa);
        if (dado != null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Erro ao atualizar",
            });
        }
        return NoContent();
    }
}
