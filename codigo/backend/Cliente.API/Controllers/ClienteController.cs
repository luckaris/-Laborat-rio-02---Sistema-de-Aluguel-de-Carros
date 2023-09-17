using Cliente.API.Interfaces;
using Cliente.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cliente.API.Controllers;

[Route("api/cliente")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly IClienteRepository _clienteRepository;
    public ClienteController(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await _clienteRepository.GetAll();
        return Ok(clientes);
    }

    [HttpGet("{cpf}")]
    public async Task<IActionResult> GetByCpf([FromRoute] string cpf)
    {
        var cliente = await _clienteRepository.GetByCpf(cpf);

        if(cliente is null)
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClienteModel cliente)
    {
        var clienteFoiCriado = await _clienteRepository.Create(cliente);
        object response;

        if(!clienteFoiCriado)
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
            nome = cliente.Nome,
        };
        return Created($"https://localhost:7247/cliente/{cliente.Cpf}", response);
    }
}
