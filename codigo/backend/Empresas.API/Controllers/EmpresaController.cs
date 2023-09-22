using Empresas.API.Models;
using Empresas.API.Models.Dto;
using Empresas.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Empresas.API.Controllers;

[Route("api/empresas")]
[ApiController]
public class EmpresaController : ControllerBase
{
    private readonly IEmpresaRepositorio _empresaRepositorio;
    public EmpresaController(IEmpresaRepositorio empresaRepositorio)
    {
        _empresaRepositorio = empresaRepositorio;
    }

    [HttpGet("{paginacao:int}")]
    public async Task<IActionResult> ObterTodos([FromRoute] int paginacao)
    {
        var empresas = await _empresaRepositorio.ObterTodos(paginacao);
        return Ok(GerarListaParaMostrarNaTela(empresas));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId([FromRoute] string id)
    {
        var empresa = await _empresaRepositorio.ObterPorId(id);
        if (empresa is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Empresa não foi encontrada."
            });
        }
        return Ok(new ListarDto()
        {
            CNPJ = empresa.CNPJ,
            Clientes = empresa.Clientes,
            Bancos = empresa.Bancos,
        });
    }

    [HttpGet("empresaId/{empresaId}")]
    public async Task<IActionResult> ObterPelaEmpresaId([FromRoute] string empresaId)
    {
        var empresa = await _empresaRepositorio.ObterPorEmpresaId(empresaId);
        if (empresa is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Empresa não foi encontrado."
            });
        }
        return Ok(new ListarDto()
        {
            CNPJ = empresa.CNPJ,
            Clientes = empresa.Clientes,
            Bancos = empresa.Bancos
        });
    }

    [HttpGet("cnpj/{cnpj}")]
    public async Task<IActionResult> ObterPeloCNPJ([FromRoute] string cnpj)
    {
        var empresa = await _empresaRepositorio.ObterPeloCNPJ(cnpj);
        if (empresa is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Empresa não foi encontrada."
            });
        }
        return Ok(new ListarDto()
        {
            CNPJ = empresa.CNPJ,
            Clientes = empresa.Clientes,
            Bancos = empresa.Bancos
        });
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarDto dto)
    {
        var cliente = await _empresaRepositorio.Cadastrar(dto);
        if (cliente == null)
        {
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Houve algum erro ao tentar cadastrar a empresa"
            });
        }
        return Created(
            $"https://localhost:7190/api/cliente/id/{cliente.Id}",
            new
            {
                status = StatusCodes.Status201Created.ToString(),
                message = "Empresa cadastrada com sucesso."
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar([FromRoute] string id, [FromBody] AtualizarDto dto)
    {
        var empresa = await _empresaRepositorio.Atualizar(id, dto);
        if (empresa == null)
        {
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Houve algum erro ao tentar atualizar os dados da empresa."
            });
        }
        return NoContent();
    }

    [HttpDelete("{cnpj}")]
    public async Task<IActionResult> Apagar([FromRoute] string cnpj)
    {
        var empresaEncontrada = await _empresaRepositorio.ObterPeloCNPJ(cnpj);
        if (empresaEncontrada is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Empresa não foi encontrada."
            });
        }
        var cliente = await _empresaRepositorio.Apagar(empresaEncontrada.Id, empresaEncontrada.EmpresaId);
        if (cliente != null)
        {
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Houve algum erro ao tentar apagar os dados da empresa."
            });
        }
        return Ok(new
        {
            status = StatusCodes.Status200OK.ToString(),
            message = "Empresa apagadA com sucesso."
        });
    }

    private static List<ListarDto> GerarListaParaMostrarNaTela(IEnumerable<EmpresaDocumento> empresas)
    {
        List<ListarDto> dtos = new();
        if (empresas.Any())
        {
            foreach (var empresa in empresas)
            {
                dtos.Add(new ListarDto
                {
                    CNPJ = empresa.CNPJ,
                    Clientes = empresa.Clientes,
                    Bancos = empresa.Bancos,
                });
            }
            return dtos;
        }
        return dtos;
    }
}
