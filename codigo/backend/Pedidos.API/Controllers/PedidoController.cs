using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.PedidosModels.Dto;
using Pedidos.API.Repository;

namespace Pedidos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        public PedidoController(IPedidoRepositorio empresaRepositorio)
        {
            _pedidoRepositorio = empresaRepositorio;
        }

        [HttpGet("{paginacao:int}")]
        public async Task<IActionResult> ObterTodos([FromRoute] int paginacao)
        {
            var pedidos = await _pedidoRepositorio.ObterTodos(paginacao);
            return Ok(pedidos);
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> ObterPeloCliente([FromRoute] string cpf)
        {
            var pedido = await _pedidoRepositorio.ObterPelaPlaca(cpf);
            if (pedido is null)
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Empresa não foi encontrada."
                });
            }
            return Ok(pedido);
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarPedidoDto dto)
        {
            var pedido = await _pedidoRepositorio.Criar(dto);
            if (pedido == null)
            {
                return BadRequest(new
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Houve algum erro ao tentar cadastrar a empresa"
                });
            }
            return Ok(pedido);
        }

        [HttpPut("{cpf}")]
        public async Task<IActionResult> Atualizar([FromRoute] string cpf, [FromBody] AtualizarPedidoDto dto)
        {
            var empresa = await _pedidoRepositorio.Atualizar(cpf, dto);
            if (empresa == null)
            {
                return BadRequest(new
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Houve algum erro ao tentar atualizar os dados do pedido."
                });
            }
            return NoContent();
        }

        [HttpDelete("{placa}")]
        public async Task<IActionResult> Apagar([FromRoute] string placa)
        {
            var pedido = await _pedidoRepositorio.ObterPelaPlaca(placa);
            if (pedido is null)
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Pedido não foi encontrado."
                });
            }
            var cliente = await _pedidoRepositorio.Apagar(pedido.CpfCliente, pedido.PlacaAutomovel);
            if (cliente != null)
            {
                return BadRequest(new
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Houve algum erro ao tentar apagar os dados do pedido."
                });
            }
            return Ok(new
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Pedido apagado com sucesso."
            });
        }
    }
}
