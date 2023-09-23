using Models.AutomoveisModels.Dto;

namespace Automoveis.API.Repository;

public interface IAutomovelRepositorio
{
    /// <summary>
    /// Obtém todos os automóveis do banco de dados ou um número específico de dados.
    /// </summary>
    /// <param name="paginacao">
    /// Número de dados que serão retornados pela API, caso seja 0, ele retornará todos,
    /// caso contrário, retornada o número passado por parâmetro.
    /// </param>
    /// <returns>List->MostrarAutomovelDto</returns>
    Task<List<MostrarAutomovelDto>> ObterTodos(int paginacao);

    /// <summary>
    /// Obtém um automóvel pelo número da placa do mesmo.
    /// </summary>
    /// <param name="placa">Placa do automóvel.</param>
    /// <returns>MostrarAutomovelDto</returns>
    Task<MostrarAutomovelDto> ObterPelaPlaca(string placa);

    /// <summary>
    /// Pesquisa automóveis de acordo com filtros.
    /// </summary>
    /// <param name="dto">Representa os filtros inseridos pelo usuário.</param>
    /// <returns>List->MostrarAutomovelDto</returns>
    Task<List<MostrarAutomovelDto>> Pesquisar(PesquisarAutomovelDto dto);

    /// <summary>
    /// Cadastra um automóvel no banco de dados.
    /// </summary>
    /// <param name="dto">Dados passados no corpo da requisição.</param>
    /// <returns>MostrarAutomovelDto</returns>
    Task<MostrarAutomovelDto> Cadastrar(CadastrarAutomovelDto dto);

    /// <summary>
    /// Atualiza os dados de um automóvel.
    /// </summary>
    /// <param name="placa">Representa a placa do automóvel.</param>
    /// <param name="dto">Dados, inseridos pelo usuário, que serão atualizados.</param>
    /// <returns></returns>
    Task<MostrarAutomovelDto> Atualizar(string placa, AtualizarAutomovelDto dto);

    /// <summary>
    /// Apagar um automóvel do banco de dados.
    /// </summary>
    /// <param name="placa">Representa a placa do automóvel.</param>
    /// <returns>MostrarAutomovelDto</returns>
    Task<MostrarAutomovelDto> Apagar(string placa);
}
