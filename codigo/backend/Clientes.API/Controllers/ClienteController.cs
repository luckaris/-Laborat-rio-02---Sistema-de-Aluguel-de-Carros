using Clientes.API.Core.Dto;
using Clientes.API.Models;
using Clientes.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clientes.API.Controllers;

[Route("api/cliente")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly IClienteRepositorio _clienteRepositorio;
    public ClienteController(IClienteRepositorio clienteRepositorio)
    {
        _clienteRepositorio = clienteRepositorio;
    }

    [HttpGet("todos/{paginacao}")]
    public async Task<IActionResult> ObterTodos([FromRoute] int paginacao)
    {
        var clientes = await _clienteRepositorio.ObterTodos(paginacao);
        return Ok(GerarListaParaMostrarNaTela(clientes));
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> ObterPorId([FromRoute] string id)
    {
        var cliente = await _clienteRepositorio.ObterPorId(id);
        if (cliente is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Cliente não foi encontrado."
            });
        }
        return Ok(GerarListaParaMostrarNaTela(new[] { cliente })[0]);
    }

    [HttpGet("usuarioId/{usuarioId}")]
    public async Task<IActionResult> ObterPeloUsuarioId([FromRoute] string usuarioId)
    {
        var cliente = await _clienteRepositorio.ObterPeloUsuarioId(usuarioId);
        if (cliente is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Cliente não foi encontrado."
            });
        }
        return Ok(GerarListaParaMostrarNaTela(new[] { cliente })[0]);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<IActionResult> ObterPeloCPF([FromRoute] string cpf)
    {
        var cliente = await _clienteRepositorio.ObterPeloCPF(cpf);
        if (cliente is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Cliente não foi encontrado."
            });
        }
        return Ok(GerarListaParaMostrarNaTela(new[] { cliente })[0]);
    }

    [HttpGet("rg/{rg}")]
    public async Task<IActionResult> ObterPeloRG([FromRoute] string rg)
    {
        var cliente = await _clienteRepositorio.ObterPeloRG(rg);
        if (cliente is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Cliente não foi encontrado."
            });
        }
        return Ok(GerarListaParaMostrarNaTela(new[] { cliente })[0]);
    }

    [HttpPost("pesquisar")]
    public async Task<IActionResult> ObterPeloNome([FromBody] PesquisarDto dto)
    {
        var clientes = await _clienteRepositorio.ObterPeloNome(dto.Nome);
        return Ok(GerarListaParaMostrarNaTela(clientes));
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarDto dto)
    {
        var cliente = await _clienteRepositorio.Criar(dto);
        if (cliente == null)
        {
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Houve algum erro ao tentar cadastrar o cliente"
            });
        }
        return Created(
            $"https://localhost:7190/api/cliente/id/{cliente.Id}",
            new
            {
                status = StatusCodes.Status201Created.ToString(),
                message = "Cliente cadastrado com sucesso."
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar([FromRoute] string id, [FromBody] AtualizarDto dto)
    {
        var cliente = await _clienteRepositorio.Atualizar(id, dto);
        if (cliente == null)
        {
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Houve algum erro ao tentar atualizar os dados do cliente."
            });
        }
        return NoContent();
    }

    [HttpDelete("{cpf}")]
    public async Task<IActionResult> Apagar([FromRoute] string cpf)
    {
        var clienteEncontrado = await _clienteRepositorio.ObterPeloCPF(cpf);
        if (clienteEncontrado is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Cliente não foi encontrado."
            });
        }
        var cliente = await _clienteRepositorio.Apagar(clienteEncontrado.Id, clienteEncontrado.UsuarioId);
        if(cliente != null)
        {
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Houve algum erro ao tentar apagar os dados do cliente."
            });
        }
        return Ok(new
        {
            status = StatusCodes.Status200OK.ToString(),
            message = "Cliente apagado com sucesso."
        });
    }

    private static List<ListarDto> GerarListaParaMostrarNaTela(IEnumerable<UsuarioDocumento> clientes)
    {
        List<ListarDto> dtos = new();
        if (clientes.Any())
        {
            foreach (var cliente in clientes)
            {
                dtos.Add(new ListarDto
                {
                    Nome = cliente.Nome,
                    RG = cliente.RG,
                    CPF = cliente.CPF,
                    Endereco = cliente.Endereco,
                    Profissao = cliente.Profissao,
                    Empregador = cliente.Empregador,
                    RendimentoMensal = cliente.RendimentoMensal
                });
            }
            return dtos;
        }
        return dtos;
    }
}
