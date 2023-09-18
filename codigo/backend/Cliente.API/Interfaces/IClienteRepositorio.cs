using Cliente.API.Core.Dto;
using Cliente.API.Models;

namespace Cliente.API.Interfaces;

public interface IClienteRepositorio
{
    /// <summary>
    /// Obtendo todos os clientes do banco de dados.
    /// </summary>
    /// <returns>Lista de Clientes.</returns>
    Task<IEnumerable<ClienteEntidade>> ObtendoTodos();

    /// <summary>
    /// Obtendo um cliente pelo CPF.
    /// </summary>
    /// <param name="cpf">Cadastro de pessoa física do cliente.</param>
    /// <returns>ClienteEntidade</returns>
    Task<ClienteEntidade> ObtendoPeloCPF(string cpf);

    /// <summary>
    /// Cadastra um usuário no banco de dados.
    /// </summary>
    /// <param name="dto">Dados digitados pelo cliente no banco de dados.</param>
    /// <returns>bool</returns>
    Task<bool> Criar(CadastrarDto dto);

    /// <summary>
    /// Atualizando os dados do cliente.
    /// </summary>
    /// <param name="cpf">Cadastro de pessoa física do cliente que terá os dados atualizados.</param>
    /// <param name="dto">Dados atualizados enviados para o cliente.</param>
    /// <returns>ClienteEntidade</returns>
    Task<ClienteEntidade> Atualizar(string cpf, AtualizarDto dto);

    /// <summary>
    /// Apagar um cliente do banco de dados.
    /// </summary>
    /// <param name="cpf">Cadastro de pessoa física do cliente a ser removido do banco de dados.</param>
    /// <returns>bool</returns>
    Task<bool> Remover(string cpf);

    /// <summary>
    /// Obtendo o endereço pelo CPF do cliente.
    /// </summary>
    /// <param name="cpf">Cadastro de pessoa física do cliente.</param>
    /// <returns>EnderecoEntidade</returns>
    Task<EnderecoEntidade> ObtendoEnderecoPeloCPF(string cpf);

    /// <summary>
    /// Obtendo o endereço do cliente pelo CEP.
    /// </summary>
    /// <param name="enderecoCEP">CEP do endereço do cliente.</param>
    /// <returns>EnderecoEntidade</returns>
    Task<EnderecoEntidade> ObtendoEnderecoPeloCEP(string enderecoCEP);
}
