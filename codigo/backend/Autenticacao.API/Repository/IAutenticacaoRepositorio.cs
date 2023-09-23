using Models.AutenticacaoModels;
using Models.AutenticacaoModels.Dtos;

namespace Autenticacao.API.Repository;

public interface IAutenticacaoRepositorio
{
    /// <summary>
    /// Esse método contém toda lógica de inserção no banco de dados,
    /// tendo métodos que verificam se um usuário já existe no banco.
    /// </summary>
    /// <param name="dto">Dados enviado pelo usuário no corpo da requisição.</param>
    /// <returns>UsuarioDocumento</returns>
    Task<UsuarioDocumento> Criar(CadastrarUsuarioDto dto);

    /// <summary>
    /// Busca um usuário de acordo com as credenciais de acesso do mesmo.
    /// </summary>
    /// <param name="dto">Dados enviados pelo usuário no corpo da requisição.</param>
    /// <returns>UsuarioDocumento</returns>
    Task<UsuarioDocumento> BuscarPelasCredenciais(LogarUsuarioDto dto);

    /// <summary>
    /// Busca um usuário pelo seu identificador
    /// </summary>
    /// <param name="identificador">Identificador de usuário</param>
    /// <returns>UsuarioDocumento</returns>
    Task<UsuarioDocumento> BuscarPeloIdentificador(string identificador);
}
