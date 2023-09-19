using Clientes.API.Core.Dto;
using Clientes.API.Models;

namespace Clientes.API.Repository;

public interface IClienteRepositorio
{
    Task<IEnumerable<UsuarioDocumento>> ObterTodos(int paginacao = 0);
    Task<UsuarioDocumento> ObterPorId(string id);
    Task<UsuarioDocumento> ObterPeloUsuarioId(string customerId);
    Task<UsuarioDocumento> ObterPeloCPF(string cpf);
    Task<UsuarioDocumento> ObterPeloRG(string rg);
    Task<IEnumerable<UsuarioDocumento>> ObterPeloNome(string nome);
    Task<UsuarioDocumento> Criar(CadastrarDto novoCliente);
    Task<UsuarioDocumento> Atualizar(string id, AtualizarDto dadosNovos);
    Task<UsuarioDocumento> Apagar(string id, string usuarioId);
}
