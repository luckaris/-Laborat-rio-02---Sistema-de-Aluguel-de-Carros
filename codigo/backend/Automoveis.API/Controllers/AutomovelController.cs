using Automoveis.API.Dto;
using Automoveis.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Automoveis.API.Controllers;

[Route("api/automoveis")]
[ApiController]
public class AutomovelController : ControllerBase
{
    private readonly IAutomovelRepositorio _automovelRepositorio;
    public AutomovelController(IAutomovelRepositorio automovelRepositorio)
    {
        _automovelRepositorio = automovelRepositorio;
    }

    [HttpGet("{paginacao:int}")]
    public async Task<IActionResult> ObterTodos([FromRoute] int paginacao)
    {
        var dados = await _automovelRepositorio.ObterTodos(paginacao);
        return Ok(dados);
    }    
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId([FromRoute] string id)
    {
        var dado = await _automovelRepositorio.ObterPorId(id);
        if(dado is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Não existe nenhum automóvel com esse Id"
            });
        }
        return Ok(dado);
    }    
    [HttpGet("automovelId/{automovelId}")]
    public async Task<IActionResult> ObterPorAutomoveId([FromRoute] string automovelId)
    {
        var dado = await _automovelRepositorio.ObterPorAutomoveId(automovelId);
        if (dado is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Não existe nenhum automóvel com esse Id de Automóvel"
            });
        }
        return Ok(dado);
    }    
    [HttpGet("placa/{placa}")]
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
    [HttpPost("pesquisar")]
    public async Task<IActionResult> Pesquisar([FromBody] PesquisarDto dto)
    {
        var dados = await _automovelRepositorio.Pesquisar(dto);
        return Ok(dados);
    }
    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarDto dto)
    {
        var dado = await _automovelRepositorio.Cadastrar(dto);
        if(dado is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Erro ao cadastrar no banco de dados",
            });
        }
        return Ok(new
        {
            status = StatusCodes.Status201Created.ToString(),
            message = "Cadastrado com sucesso"
        });
    }    
    [HttpPut("{placa")]
    public async Task<IActionResult> Atualizar([FromRoute] string placa, [FromBody] AtualizarDto dto)
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
        return Ok(new
        {
            status = StatusCodes.Status201Created.ToString(),
            message = "Cadastrado com sucesso"
        });
    }    
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
        return Ok(new
        {
            status = StatusCodes.Status201Created.ToString(),
            message = "Cadastrado com sucesso"
        });
    }
}
