using Microsoft.AspNetCore.Mvc;
using Pedidos.API.Models;
using Pedidos.API.Service;

namespace Pedidos.API.Controllers;

[Route("api/pedidos")]
[ApiController]
public class PedidoController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> EnviarPedido([FromServices] ProducerService producerService, [FromBody] Pedido pedido)
    {
        var mensagem = await producerService.SendMessage(pedido);

        if(mensagem.Equals("Erro ao enviar mensagem"))
        {
            return BadRequest(new
            {
                mensagem
            });
        }
        return Ok(new
        {
            mensagem
        });
    }
}
