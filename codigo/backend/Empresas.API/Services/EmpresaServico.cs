using Empresas.API.Models;
using Empresas.API.Models.Dto;
using Empresas.API.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Empresas.API.Services;

public class EmpresaServico : IEmpresaRepositorio
{
    private readonly CosmosClient _cosmosClient;
    private readonly IConfiguration _configuracao;
    private readonly Container _container;
    public EmpresaServico(CosmosClient cosmosClient, IConfiguration configuracao)
    {
        _cosmosClient = cosmosClient;
        _configuracao = configuracao;

        string nomeDoBancoDeDados = _configuracao["AzureCosmosDbSettings:DatabaseName"]!;
        var nomeDoContainer = "Empresas";
        _container = _cosmosClient.GetContainer(nomeDoBancoDeDados, nomeDoContainer);
    }

    public async Task<List<EmpresaDocumento>> ObterTodos(int paginacao)
    {
        try
        {
            QueryDefinition consulta;

            if (paginacao == 0)
            {
                consulta = _container.GetItemLinqQueryable<EmpresaDocumento>().ToQueryDefinition();
            }
            else
            {
                consulta = _container.GetItemLinqQueryable<EmpresaDocumento>().Take(paginacao).ToQueryDefinition();
            }

            List<EmpresaDocumento> empresas = new();
            var response = await _container.GetItemQueryIterator<EmpresaDocumento>(consulta).ReadNextAsync();

            empresas.AddRange(response);
            return empresas;
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao consultar todas as empresas.");
        }
    }
    
    public async Task<string> ObterId(string credencial)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<EmpresaDocumento>()
                .Where(c => c.CNPJ.Equals(credencial)).ToFeedIterator();
            var clientes = new List<EmpresaDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                clientes.AddRange(resposta);
            }

            var cliente = clientes.FirstOrDefault()!;
            return cliente.Id;
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao consultar empresa pelo ID.");
        }
    }

    public async Task<EmpresaDocumento> ObterPelasCredenciais(string credencial)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<EmpresaDocumento>()
                .Where(c => c.CNPJ.Equals(credencial)).ToFeedIterator();
            var empresas = new List<EmpresaDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                empresas.AddRange(resposta);
            }

            var cliente = empresas.FirstOrDefault()!;
            return cliente;
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao consultar empresa pelo CNPJ.");
        }
    }

    public async Task<EmpresaDocumento> Cadastrar(CadastrarDto empresa)
    {
        try
        {
            var empresaEncontrada = await ObterPelasCredenciais(empresa.CNPJ);
            if (empresaEncontrada != null) return null!;

            string guid = Guid.NewGuid().ToString();
            var cliente = new EmpresaDocumento()
            {
                Id = guid,
                EmpresaId = guid,
                Nome = empresa.Nome,
                CNPJ = empresa.CNPJ,
                Clientes = new List<ClienteDto>(),
                Bancos = new List<BancoDto>()
            };
            var reposta = await _container.CreateItemAsync(cliente);
            return reposta.Resource;
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao cadastrar uma empresa.");
        }
    }

    public async Task<EmpresaDocumento> Atualizar(string credencial, AtualizarDto documento)
    {
        try
        {
            string id = await ObterId(credencial);
            var empresa = new EmpresaDocumento()
            {
                Id = id,
                EmpresaId = id,
                Nome= documento.Nome,
                CNPJ = documento.CNPJ,
                Clientes = documento.Clientes,
                Bancos = documento.Bancos
            };
            var resposta = await _container.ReplaceItemAsync(empresa, id);
            return resposta.Resource;
        }
        catch
        {
            throw new Exception("Houve um erro ao atualizar os dados de uma empresa");
        }
    }
    
    public async Task<EmpresaDocumento> Apagar(string credencial)
    {
        try
        {
            string id = await ObterId(credencial);
            var resposta = await _container.DeleteItemAsync<EmpresaDocumento>(id, new PartitionKey(id));
            return resposta.Resource;
        }
        catch
        {
            throw new Exception("Houve um erro ao apagar uma empresa.");
        }
    }
}
