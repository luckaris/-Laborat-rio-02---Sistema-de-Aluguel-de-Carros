using Automoveis.API.Dto;
using Automoveis.API.Models;
using Automoveis.API.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json;
using System.Numerics;

namespace Automoveis.API.Services;

public class AutomovelServico : IAutomovelRepositorio
{
    private readonly CosmosClient _cosmosClient;
    private readonly IConfiguration _configuracao;
    private readonly Container _container;
    public AutomovelServico(CosmosClient cosmosClient, IConfiguration configuracao)
    {
        _cosmosClient = cosmosClient;
        _configuracao = configuracao;

        string nomeDoBancoDeDados = _configuracao["AzureCosmosDbSettings:DatabaseName"]!;
        var nomeDoContainer = "Automoveis";
        _container = _cosmosClient.GetContainer(nomeDoBancoDeDados, nomeDoContainer);
    }
    public async Task<List<ListarDto>> ObterTodos(int paginacao)
    {
        try
        {
            QueryDefinition consulta;

            if (paginacao == 0)
            {
                consulta = _container.GetItemLinqQueryable<AutomovelDocumento>().ToQueryDefinition();
            }
            else
            {
                consulta = _container.GetItemLinqQueryable<AutomovelDocumento>().Take(paginacao).ToQueryDefinition();
            }

            List<ListarDto> automoveis = new();
            var response = await _container.GetItemQueryIterator<ListarDto>(consulta).ReadNextAsync();

            automoveis.AddRange(response);
            return automoveis;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar todas as empresas.");
        }
    }

    public async Task<ListarDto> ObterPorId(string id)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<AutomovelDocumento>()
                .Where(c => c.Id.Equals(id)).ToFeedIterator();
            var automoveis = new List<AutomovelDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                automoveis.AddRange(resposta);
            }

            var automovel = automoveis.FirstOrDefault()!;
            return new ListarDto()
            {
                Renavam = automovel.Renavam,
                Ano = automovel.Ano,
                Marca = automovel.Marca,
                Modelo = automovel.Modelo,
                Placa = automovel.Placa,
                Mensalidade = automovel.Mensalidade
            };
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar automóvel pelo ID.");
        }
    }

    public async Task<ListarDto> ObterPorAutomoveId(string automovelId)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<AutomovelDocumento>()
                .Where(c => c.AutomovelId.Equals(automovelId)).ToFeedIterator();
            var automoveis = new List<AutomovelDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                automoveis.AddRange(resposta);
            }

            var automovel = automoveis.FirstOrDefault()!;
            return new ListarDto()
            {
                Renavam = automovel.Renavam,
                Ano = automovel.Ano,
                Marca = automovel.Marca,
                Modelo = automovel.Modelo,
                Placa = automovel.Placa,
                Mensalidade = automovel.Mensalidade
            };
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar automóvel pelo ID.");
        }
    }

    public async Task<ListarDto> ObterPelaPlaca(string placa)
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
            return new ListarDto()
            {
                Renavam = automovel.Renavam,
                Ano = automovel.Ano,
                Marca = automovel.Marca,
                Modelo = automovel.Modelo,
                Placa = automovel.Placa,
                Mensalidade = automovel.Mensalidade
            };
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar automóvel pela placa.");
        }
    }

    public async Task<List<ListarDto>> Pesquisar(PesquisarDto dto)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<AutomovelDocumento>()
                .Where(c => c.Modelo.Contains(dto.Modelo)).ToFeedIterator();
            var automoveis = new List<AutomovelDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                automoveis.AddRange(resposta);
            }

            return GerarListaDto(automoveis);
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao pesquisar automóvel pelo modelo.");
        }
    }

    public async Task<ListarDto> Cadastrar(CadastrarDto dto)
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
                Mensalidade = dto.Mensalidade
            };
            var reposta = await _container.CreateItemAsync(automovel);
            if (reposta.Resource is null) return null!;
            return new ListarDto()
            {
                Renavam = automovel.Renavam,
                Ano = automovel.Ano,
                Marca = automovel.Marca,
                Modelo = automovel.Modelo,
                Placa = automovel.Placa,
                Mensalidade = automovel.Mensalidade
            };
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao cadastrar um automóvel.");
        }
    }

    public async Task<ListarDto> Atualizar(string placa, AtualizarDto dto)
    {
        try
        {
            string id = await GetIdAsync(placa);
            var automovel = new AutomovelDocumento()
            {
                Id = id,
                AutomovelId = id,
                Renavam = dto.Renavam,
                Ano = dto.Ano,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Placa = dto.Placa,
                Mensalidade = dto.Mensalidade
            };
            var resposta = await _container.ReplaceItemAsync(automovel, id);
            return new ListarDto()
            {
                Renavam = resposta.Resource.Renavam,
                Ano = resposta.Resource.Ano,
                Marca = resposta.Resource.Marca,
                Modelo = resposta.Resource.Modelo,
                Placa = resposta.Resource.Placa,
                Mensalidade = resposta.Resource.Mensalidade
            };
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao atualizar os dados de um automóvel");
        }
    }

    public async Task<ListarDto> Apagar(string placa)
    {
        try
        {
            string id = await GetIdAsync(placa);
            var resposta = await _container.DeleteItemAsync<AutomovelDocumento>(id, new PartitionKey(id));
            return new ListarDto()
            {
                Renavam = resposta.Resource.Renavam,
                Ano = resposta.Resource.Ano,
                Marca = resposta.Resource.Marca,
                Modelo = resposta.Resource.Modelo,
                Placa = resposta.Resource.Placa,
                Mensalidade = resposta.Resource.Mensalidade
            };
        }
        catch (Exception e)
        {
            throw new Exception($"Erro ao apagar uma empresa. Detalhes técnicos: {e.Message}");
        }
    }

    private async Task<string> GetIdAsync(string placa)
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
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar automóvel pela placa.");
        }
    }
    
    private List<ListarDto> GerarListaDto(List<AutomovelDocumento> lista)
    {
        var dtos = new List<ListarDto>();
        foreach(var item in lista)
        {
            dtos.Add(new ListarDto()
            {
                Renavam = item.Renavam,
                Ano = item.Ano,
                Marca = item.Marca,
                Modelo = item.Modelo,
                Placa = item.Placa,
                Mensalidade = item.Mensalidade
            });
        }
        return dtos;
    }
}
