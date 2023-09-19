using Clientes.API.Core.Dto;
using Clientes.API.Models;
using Clientes.API.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Clientes.API.Services;

public class ClienteServico : IClienteRepositorio
{
    private readonly CosmosClient _cosmosClient;
    private readonly IConfiguration _configuracao;
    private readonly Container _container;
    public ClienteServico(CosmosClient cosmosClient, IConfiguration configuracao)
    {
        _cosmosClient = cosmosClient;
        _configuracao = configuracao;

        string nomeDoBancoDeDados = _configuracao["AzureCosmosDbSettings:DatabaseName"]!;
        var nomeDoContainer = "Usuarios";
        _container = _cosmosClient.GetContainer(nomeDoBancoDeDados, nomeDoContainer);
    }

    public async Task<IEnumerable<UsuarioDocumento>> ObterTodos(int paginacao = 0)
    {
        try
        {
            QueryDefinition consulta;

            if (paginacao == 0)
                consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
                    .ToQueryDefinition();
            else
                consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
                    .Take(paginacao).ToQueryDefinition();

            var response = await _container.GetItemQueryIterator<UsuarioDocumento>(consulta).ReadNextAsync();
            return response;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar todos os clientes.");
        }
    }

    public async Task<UsuarioDocumento> ObterPorId(string id)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
                .Where(c => c.Id.Equals(id)).ToFeedIterator();
            var clientes = new List<UsuarioDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                clientes.AddRange(resposta);
            }

            var cliente = clientes.FirstOrDefault()!;
            return cliente;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar cliente pelo ID.");
        }
    }

    public async Task<UsuarioDocumento> ObterPeloUsuarioId(string usuarioId)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
            .Where(c => c.UsuarioId.Equals(usuarioId)).ToFeedIterator();
            var clientes = new List<UsuarioDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                clientes.AddRange(resposta);
            }

            var cliente = clientes.FirstOrDefault()!;
            return cliente;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar cliente pelo ID de Cliente.");
        }
    }

    public async Task<UsuarioDocumento> ObterPeloCPF(string cpf)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
            .Where(c => c.CPF.Equals(cpf)).ToFeedIterator();
            var clientes = new List<UsuarioDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                clientes.AddRange(resposta);
            }

            var cliente = clientes.FirstOrDefault()!;
            return cliente;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar cliente pelo ID de Cliente.");
        }
    }

    public async Task<UsuarioDocumento> ObterPeloRG(string rg)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
            .Where(c => c.RG.Equals(rg)).ToFeedIterator();
            var clientes = new List<UsuarioDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                clientes.AddRange(resposta);
            }

            var cliente = clientes.FirstOrDefault()!;
            return cliente;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar cliente pelo ID de Cliente.");
        }
    }

    public async Task<IEnumerable<UsuarioDocumento>> ObterPeloNome(string nome)
    {
        try
        {
            var query = _container.GetItemLinqQueryable<UsuarioDocumento>()
                .Where(c => c.Nome.Contains(nome)).ToFeedIterator();
            var clientes = new List<UsuarioDocumento>();

            while (query.HasMoreResults)
            {
                var resposta = await query.ReadNextAsync();
                clientes.AddRange(resposta);
            }

            return clientes;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao consultar cliente pelo nome.");
        }
    }

    public async Task<UsuarioDocumento> Criar(CadastrarDto novoCliente)
    {
        try
        {
            var clienteEncontrado = await ObterPeloCPF(novoCliente.CPF);
            if(clienteEncontrado != null) return null!;

            var cliente = new UsuarioDocumento()
            {
                Nome = novoCliente.Nome,
                RG = novoCliente.RG,
                CPF = novoCliente.CPF,
                Senha = novoCliente.Senha,
                Endereco = novoCliente.Endereco,
                Profissao = novoCliente.Profissao,
                Empregador = novoCliente.Empregador,
                RendimentoMensal = novoCliente.RendimentoMensal
            };
            var reposta = await _container.CreateItemAsync(cliente);
            return reposta.Resource;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao cadastrar um cliente.");
        }
    }

    public async Task<UsuarioDocumento> Atualizar(string id, AtualizarDto dadosNovos)
    {
        try
        {
            var cliente = new UsuarioDocumento(id)
            {
                Nome = dadosNovos.Nome,
                RG = dadosNovos.RG,
                CPF = dadosNovos.CPF,
                Senha = dadosNovos.Senha,
                Endereco = dadosNovos.Endereco,
                Profissao = dadosNovos.Profissao,
                Empregador = dadosNovos.Empregador,
                RendimentoMensal = dadosNovos.RendimentoMensal
            };
            var resposta = await _container.ReplaceItemAsync(cliente, id);
            return resposta.Resource;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao atualizar os dados de um cliente");
        }
    }

    public async Task<UsuarioDocumento> Apagar(string id, string usuarioId)
    {
        try
        {
            var resposta = await _container.DeleteItemAsync<UsuarioDocumento>(id, new PartitionKey(usuarioId));
            return resposta.Resource;
        }
        catch (Exception e)
        {
            throw new Exception($"Erro ao apagar um cliente. Detalhes técnicos: {e.Message}");
        }
    }
}
