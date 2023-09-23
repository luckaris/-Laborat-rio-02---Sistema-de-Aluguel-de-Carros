using Automoveis.API.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Models.AutomoveisModels;
using Models.AutomoveisModels.Dto;
using System.Reflection.Metadata.Ecma335;

namespace Automoveis.API.Services;

public class AutomovelServico : IAutomovelRepositorio
{
    private readonly CosmosClient _cosmosClient;
    private readonly IConfiguration _configuracao;
    private readonly Container _container;
    /// <summary>
    /// O construtor dessa classe realiza a Injeção de Dependência da classe que implementa da Interface na 
    /// qual a mesma está sendo passada por parâmetro, essa configuração é definina da classe Program, onde
    /// chamamos o método AddScoped.
    /// <br></br>>
    /// A mesma configura qual Container do Cosmos iremos usar para esse serviço.
    /// </summary>
    /// <param name="cosmosClient">Classe do CosmosClient na qual acessaremos o banco de dados.</param>
    /// <param name="configuracao">Interface para obter dados do appsettings.</param>
    public AutomovelServico(CosmosClient cosmosClient, IConfiguration configuracao)
    {
        _cosmosClient = cosmosClient;
        _configuracao = configuracao;

        string nomeDoBancoDeDados = _configuracao["AzureCosmosDbSettings:DatabaseName"]!;
        var nomeDoContainer = "Automoveis";
        _container = _cosmosClient.GetContainer(nomeDoBancoDeDados, nomeDoContainer);
    }

    public async Task<List<MostrarAutomovelDto>> ObterTodos(int paginacao)
    {
        try
        {
            QueryDefinition consulta;

            if (paginacao == 0)
                consulta = _container.GetItemLinqQueryable<AutomovelDocumento>().ToQueryDefinition();
            else
                consulta = _container.GetItemLinqQueryable<AutomovelDocumento>().Take(paginacao).ToQueryDefinition();

            List<MostrarAutomovelDto> automoveis = new();
            var response = await _container.GetItemQueryIterator<MostrarAutomovelDto>(consulta).ReadNextAsync();

            automoveis.AddRange(response);
            return automoveis;
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao tentar consultar todos os automóveis.");
        }
    }

    public async Task<MostrarAutomovelDto> ObterPelaPlaca(string placa)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<AutomovelDocumento>()
                .Where(c => c.Placa.Equals(placa)).ToFeedIterator();
            var automoveis = new List<AutomovelDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                automoveis.AddRange(resposta);
            }

            var automovel = automoveis.FirstOrDefault()!;
            if (automovel is null) return null!;
            return new MostrarAutomovelDto()
            {
                Ano = automovel.Ano,
                Marca = automovel.Marca,
                Modelo = automovel.Modelo,
                Placa = automovel.Placa,
                Mensalidade = automovel.Mensalidade,
                Status = automovel.Status,
            };
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao tentar consultar automóvel pela placa.");
        }
    }

    public async Task<List<MostrarAutomovelDto>> Pesquisar(PesquisarAutomovelDto dto)
    {
        try
        {
            FeedIterator<AutomovelDocumento?> consulta = FiltrarAutomoveis(dto);

            var automoveis = new List<AutomovelDocumento>();
            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                automoveis.AddRange(resposta!);
            }
            return GerarListaDto(automoveis);
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro durante o filtro de automóveis.");
        }
    }

    public async Task<MostrarAutomovelDto> Cadastrar(CadastrarAutomovelDto dto)
    {
        try
        {
            var automovelEncontrado = await ObterPelaPlaca(dto.Placa);
            if (automovelEncontrado != null) return null!;

            string guid = Guid.NewGuid().ToString();
            var automovel = new AutomovelDocumento()
            {
                Id = guid,
                AutomovelId = guid,
                Renavam = dto.Renavam,
                Ano = dto.Ano,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Placa = dto.Placa,
                Mensalidade = dto.Mensalidade,
                Status = "DISPONIVEL"
            };
            var reposta = await _container.CreateItemAsync(automovel);
            if (reposta.Resource is null) return null!;
            return new MostrarAutomovelDto()
            {
                Ano = automovel.Ano,
                Marca = automovel.Marca,
                Modelo = automovel.Modelo,
                Placa = automovel.Placa,
                Mensalidade = automovel.Mensalidade,
                Status = automovel.Status,
            };
        }
        catch (Exception)
        {
            throw new Exception("Houve erro ao cadastrar um automóvel.");
        }
    }

    public async Task<MostrarAutomovelDto> Atualizar(string placa, AtualizarAutomovelDto dto)
    {
        try
        {
            string id = await ObterId(placa);
            var automovel = new AutomovelDocumento()
            {
                Id = id,
                AutomovelId = id,
                Renavam = dto.Renavam,
                Ano = dto.Ano,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Placa = dto.Placa,
                Mensalidade = dto.Mensalidade,
                Status = dto.Status,
            };
            var resposta = await _container.ReplaceItemAsync(automovel, id);
            return new MostrarAutomovelDto()
            {
                Ano = resposta.Resource.Ano,
                Marca = resposta.Resource.Marca,
                Modelo = resposta.Resource.Modelo,
                Placa = resposta.Resource.Placa,
                Mensalidade = resposta.Resource.Mensalidade,
                Status = dto.Status,
            };
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao atualizar os dados de um automóvel");
        }
    }

    public async Task<MostrarAutomovelDto> Apagar(string placa)
    {
        try
        {
            string id = await ObterId(placa);
            var resposta = await _container.DeleteItemAsync<AutomovelDocumento>(id, new PartitionKey(id));
            if(resposta.Resource == null) return null!;
            
            return new MostrarAutomovelDto()
            {
                Ano = resposta.Resource.Ano,
                Marca = resposta.Resource.Marca,
                Modelo = resposta.Resource.Modelo,
                Placa = resposta.Resource.Placa,
                Mensalidade = resposta.Resource.Mensalidade
            };
        }
        catch
        {
            throw new Exception("Houve um erro ao apagar o automóvel.");
        }
    }

    /// <summary>
    /// Obtém o Id de um veículo.
    /// </summary>
    /// <param name="placa">Placa do veículo.</param>
    /// <returns>string</returns>
    /// <exception cref="Exception"></exception>
    private async Task<string> ObterId(string placa)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<AutomovelDocumento>()
                .Where(c => c.Placa.Equals(placa)).ToFeedIterator();
            var automoveis = new List<AutomovelDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                automoveis.AddRange(resposta);
            }

            var automovel = automoveis.FirstOrDefault()!;
            return automovel.Id;
        }
        catch
        {
            throw new Exception("Houve um erro ao consultar automóvel pela placa.");
        }
    }

    /// <summary>
    /// Realiza filtros para obter automóveis específicos.
    /// </summary>
    /// <param name="dto">Dados do filtro passado no corpo da requisição.</param>
    /// <returns>FeedIterator->AutomovelDocumento</returns>
    private FeedIterator<AutomovelDocumento?> FiltrarAutomoveis(PesquisarAutomovelDto dto)
    {
        FeedIterator <AutomovelDocumento?> consulta;

        if (dto.AnoInicial == 0 && dto.AnoFinal == 0)
        {
            dto.AnoInicial = DateTime.Now.AddYears(-50).Year;
            dto.AnoFinal = DateTime.Now.Year;
        }
        else if (dto.AnoInicial > dto.AnoFinal)
        {
            (dto.AnoInicial, dto.AnoFinal) = (dto.AnoFinal, dto.AnoInicial);
        }

        if (dto.Status.Count <= 0)
        {
            dto.Status.Add("DISPONIVEL");
        }

        if (string.IsNullOrEmpty(dto.Modelo) && string.IsNullOrEmpty(dto.Marca))
        {
            consulta = _container.GetItemLinqQueryable<AutomovelDocumento>()
                .Where(c =>
                    (c.Ano < dto.AnoFinal && c.Ano > dto.AnoInicial) &&
                    (dto.Status.Contains(c.Status)))
                .ToFeedIterator()!;
        }
        else if (string.IsNullOrEmpty(dto.Modelo))
        {
            consulta = _container.GetItemLinqQueryable<AutomovelDocumento>()
            .Where(c =>
                (c.Marca.ToLower().Contains(dto.Marca.ToLower())) &&
                (c.Ano < dto.AnoFinal && c.Ano > dto.AnoInicial) &&
                (dto.Status.Contains(c.Status)))
            .ToFeedIterator()!;
        }
        else if (string.IsNullOrEmpty(dto.Marca))
        {
            consulta = _container.GetItemLinqQueryable<AutomovelDocumento>()
            .Where(c =>
                (c.Modelo.ToLower().Contains(dto.Modelo.ToLower())) &&
                (c.Ano <= dto.AnoFinal && c.Ano >= dto.AnoInicial) &&
                (dto.Status.Contains(c.Status)))
            .ToFeedIterator()!;
        }
        else
        {
            consulta = _container.GetItemLinqQueryable<AutomovelDocumento>()
            .Where(c =>
                (c.Modelo.ToLower().Contains(dto.Modelo.ToLower())) &&
                (c.Marca.ToLower().Contains(dto.Marca.ToLower())) &&
                (c.Ano < dto.AnoFinal && c.Ano > dto.AnoInicial) &&
                (dto.Status.Contains(c.Status)))
            .ToFeedIterator()!;
        }

        return consulta;
    }

    /// <summary>
    /// Retorna uma lista de automóveis formatados para retorno.
    /// </summary>
    /// <param name="lista">Lista de automóveis obtidos por consulta.</param>
    /// <returns>List->MostrarAutomovelDto</returns>
    private static List<MostrarAutomovelDto> GerarListaDto(List<AutomovelDocumento> lista)
    {
        var dtos = new List<MostrarAutomovelDto>();
        foreach (var item in lista)
        {
            dtos.Add(new MostrarAutomovelDto()
            {
                Ano = item.Ano,
                Marca = item.Marca,
                Modelo = item.Modelo,
                Placa = item.Placa,
                Mensalidade = item.Mensalidade,
                Status = item.Status,
            });
        }
        return dtos;
    }
}
