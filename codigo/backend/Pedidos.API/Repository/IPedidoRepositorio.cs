using Models.PedidosModels;
using Models.PedidosModels.Dto;

namespace Pedidos.API.Repository;

public interface IPedidoRepositorio
{
    Task<List<MostrarPedidoDto>> ObterTodos(int paginacao);
    Task<PedidoDocumento> ObterPelaPlaca(string cpf);
    Task<List<MostrarPedidoDto>> ObterPeloStatus(string status);
    Task<MostrarPedidoDto> Criar(CadastrarPedidoDto dto);
    Task<MostrarPedidoDto> Atualizar(string cpf, AtualizarPedidoDto dto);
    Task<MostrarPedidoDto> Apagar(string placa);
}
