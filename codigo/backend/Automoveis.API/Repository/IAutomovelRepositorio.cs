using Automoveis.API.Dto;

namespace Automoveis.API.Repository;

public interface IAutomovelRepositorio
{
    Task<List<ListarDto>> ObterTodos(int paginacao);
    Task<ListarDto> ObterPorId(string id);
    Task<ListarDto> ObterPorAutomoveId(string automovelId);
    Task<ListarDto> ObterPelaPlaca(string placa);
    Task<List<ListarDto>> Pesquisar(PesquisarDto dto);
    Task<ListarDto> Cadastrar(CadastrarDto dto);
    Task<ListarDto> Atualizar(string placa, AtualizarDto dto);
    Task<ListarDto> Apagar(string placa);
}
