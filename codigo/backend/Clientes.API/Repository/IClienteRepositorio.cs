using Models.ClientesModels;
using Models.ClientesModels.Dto;

namespace Clientes.API.Repository;

public interface IClienteRepositorio
{
    Task<IEnumerable<ClienteDocumento>> ObterTodos(int paginacao = 0);
    Task<ClienteDocumento> ObterPelasCredenciais(string credencial);
    Task<IEnumerable<ClienteDocumento>> ObterPeloNome(string nome);
    Task<ClienteDocumento> Criar(CadastrarClienteDto dto);
    Task<ClienteDocumento> Atualizar(string credencial, AtualizarClienteDto dto);
    Task<ClienteDocumento> Apagar(string credencial);
}
