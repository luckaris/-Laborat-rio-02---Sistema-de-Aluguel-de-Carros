using Cliente.API.Core.Dto;
using Cliente.API.Interfaces;
using Cliente.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cliente.API.Controllers;

[Route("api/cliente")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly IClienteRepositorio _clienteRepository;
    public ClienteController(IClienteRepositorio clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await _clienteRepository.ObtendoTodos();
        return Ok(clientes);
    }

    [HttpGet("{cpf}")]
    public async Task<IActionResult> GetByCpf([FromRoute] string cpf)
    {
        var cliente = await _clienteRepository.ObtendoPeloCPF(cpf);

        if (cliente is null)
        {
            var response = new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Usuário não foi encontrado no banco de dados"
            };
            return NotFound(response);
        }

        return Ok(cliente);
    }

    [HttpGet("endereco/{cep}")]
    public async Task<IActionResult> ObtendoEnderecoPeloCEP([FromRoute] string cep)
    {
        var endereco = await _clienteRepository.ObtendoEnderecoPeloCEP(cep);

        if (endereco is null)
        {
            var response = new
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Endereço não foi encontrado no banco de dados"
            };
            return NotFound(response);
        }
        return Ok(endereco);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CadastrarDto dto)
    {
        object response;
        var clienteFoiCriado = await _clienteRepository.Criar(dto);
        if (!clienteFoiCriado)
        {
            response = new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Erro ao criar cliente no banco de dados"
            };
            return BadRequest(response);
        }
        response = new
        {
            nome = dto.Nome,
        };
        return Created($"https://localhost:7247/cliente/{dto.CPF}", response);
    }

    [HttpPut("{cpf}")]
    public async Task<IActionResult> Update([FromRoute] string cpf, [FromBody] AtualizarDto dto)
    {
        var clienteAtualizado = await _clienteRepository.Atualizar(cpf, dto);
        if (clienteAtualizado is null)
        {
            var response = new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Erro ao atualizar os dados do cliente."
            };
            return BadRequest(response);
        }
        return NoContent();
    }

    [HttpDelete("{cpf}")]
    public async Task<IActionResult> Detele([FromRoute] string cpf)
    {
        var clienteFoiDeletado = await _clienteRepository.Remover(cpf);
        if (!clienteFoiDeletado)
        {
            var response = new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Erro ao apagar cliente do banco de dados."
            };
            return BadRequest(response);
        }
        return NoContent();
    }
}
