using Clientes.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Models.ClientesModels;
using Models.ClientesModels.Dto;

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

    [HttpGet("todos/{paginacao:int}")]
    public async Task<IActionResult> ObterTodos([FromRoute] int paginacao)
    {
        var clientes = await _clienteRepositorio.ObterTodos(paginacao);
        return Ok(GerarListaParaMostrarNaTela(clientes.ToList()));
    }

    [HttpGet("{credencial}")]
    public async Task<IActionResult> ObterPelaCredencial([FromRoute] string credencial)
    {
        var cliente = await _clienteRepositorio.ObterPelasCredenciais(credencial);
        if (cliente is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Cliente não foi encontrado."
            });
        }
        return Ok(new ListarDto()
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

    [HttpPost("pesquisar")]
    public async Task<IActionResult> ObterPeloNome([FromBody] PesquisarClienteDto dto)
    {
        var clientes = await _clienteRepositorio.ObterPeloNome(dto.Nome);
        return Ok(GerarListaParaMostrarNaTela(clientes.ToList()));
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarClienteDto dto)
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

    [HttpPut("{cpf}")]
    public async Task<IActionResult> Atualizar([FromRoute] string cpf, [FromBody] AtualizarClienteDto dto)
    {
        var cliente = await _clienteRepositorio.Atualizar(cpf, dto);
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
        var clienteEncontrado = await _clienteRepositorio.ObterPelasCredenciais(cpf);
        if (clienteEncontrado is null)
        {
            return NotFound(new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Cliente não foi encontrado."
            });
        }
        var cliente = await _clienteRepositorio.Apagar(cpf);
        if (cliente != null)
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

    /// <summary>
    /// Gera lista de clientes para mostrar na tela.
    /// </summary>
    /// <param name="clientes">Clientes obtidos no banco de dados.</param>
    /// <returns>List->ListarDto</returns>
    private static List<ListarDto> GerarListaParaMostrarNaTela(List<ClienteDocumento> clientes)
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
                    RendimentoMensal = cliente.RendimentoMensal,
                });
            }
            return dtos;
        }
        return dtos;
    }
}
