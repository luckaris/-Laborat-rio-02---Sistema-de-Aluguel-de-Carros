using Autenticacao.API.Repository;
using Clientes.API.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Models.AutenticacaoModels.Dtos;
using Models.ClientesModels;
using Models.ClientesModels.Dto;

namespace Clientes.API.Services;

public class ClienteServico : IClienteRepositorio
{
    private readonly CosmosClient _cosmosClient;
    private readonly IConfiguration _configuracao;
    private readonly Container _containerCliente;
    private readonly IAutenticacaoRepositorio _usuarioRepositorio;
    public ClienteServico(IAutenticacaoRepositorio usuario, CosmosClient cosmosClient, IConfiguration configuracao)
    {
        _cosmosClient = cosmosClient;
        _configuracao = configuracao;
        _usuarioRepositorio = usuario;

        string nomeDoBancoDeDados = _configuracao["AzureCosmosDbSettings:DatabaseName"]!;
        var nomeDoContainer = "Clientes";
        _containerCliente = _cosmosClient.GetContainer(nomeDoBancoDeDados, nomeDoContainer);
    }

    public async Task<IEnumerable<ClienteDocumento>> ObterTodos(int paginacao = 0)
    {
        try
        {
            QueryDefinition consulta;

            if (paginacao == 0)
                consulta = _containerCliente.GetItemLinqQueryable<ClienteDocumento>()
                    .ToQueryDefinition();
            else
                consulta = _containerCliente.GetItemLinqQueryable<ClienteDocumento>()
                    .Take(paginacao).ToQueryDefinition();

            var response = await _containerCliente.GetItemQueryIterator<ClienteDocumento>(consulta).ReadNextAsync();
            return response;
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao consultar todos os clientes.");
        }
    }

    public async Task<ClienteDocumento> ObterPelasCredenciais(string credencial)
    {
        try
        {
            var consulta = _containerCliente.GetItemLinqQueryable<ClienteDocumento>()
            .Where(c => c.CPF.Equals(credencial) || c.RG.ToLower().Equals(credencial)).ToFeedIterator();
            var clientes = new List<ClienteDocumento>();

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

    public async Task<IEnumerable<ClienteDocumento>> ObterPeloNome(string nome)
    {
        try
        {
            var query = _containerCliente.GetItemLinqQueryable<ClienteDocumento>()
                .Where(c => c.Nome.Contains(nome)).ToFeedIterator();
            var clientes = new List<ClienteDocumento>();

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

    public async Task<ClienteDocumento> Criar(CadastrarClienteDto dto)
    {
        try
        {
            ClienteDocumento cliente;
            var clienteEncontrado = await ObterPelasCredenciais(dto.CPF)
                ?? await ObterPelasCredenciais(dto.RG);
            if (clienteEncontrado != null) return null!;

            var usuario = await _usuarioRepositorio.BuscarPeloIdentificador(dto.CPF);
            if (usuario != null)
            {
                cliente = new ClienteDocumento()
                {
                    Id = usuario.Id,
                    ClienteId = usuario.UsuarioId,
                    Nome = usuario.Nome,
                    RG = dto.RG,
                    CPF = usuario.Identificador,
                    Endereco = dto.Endereco,
                    Profissao = dto.Profissao,
                    Empregador = dto.Empregador,
                    RendimentoMensal = dto.RendimentoMensal,
                    Permissao = usuario.Tipo
                };
            }
            else
            {
                string guid = Guid.NewGuid().ToString();
                cliente = new ClienteDocumento()
                {
                    Id = guid,
                    ClienteId = guid,
                    Nome = dto.Nome,
                    RG = dto.RG,
                    CPF = dto.CPF,
                    Endereco = dto.Endereco,
                    Profissao = dto.Profissao,
                    Empregador = dto.Empregador,
                    RendimentoMensal = dto.RendimentoMensal,
                    Permissao = EPermissaoAcesso.CLIENTE.ToString(),
                };
                _ = await _usuarioRepositorio.Criar(new CadastrarUsuarioDto()
                {
                    Nome = dto.Nome,
                    Identificador = dto.CPF,
                    Senha = dto.Senha,
                    Tipo = EPermissaoAcesso.CLIENTE.ToString()
                });
            }

            var reposta = await _containerCliente.CreateItemAsync(cliente);
            return reposta.Resource;
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao cadastrar um cliente.");
        }
    }

    public async Task<ClienteDocumento> Atualizar(string cpf, AtualizarClienteDto dto)
    {
        try
        {
            ClienteDocumento cliente;
            var usuario = await _usuarioRepositorio.BuscarPeloIdentificador(cpf);

            if (usuario != null)
            {
                cliente = new ClienteDocumento()
                {
                    Id = usuario.Id,
                    ClienteId = usuario.UsuarioId,
                    Nome = dto.Nome,
                    RG = dto.RG,
                    CPF = dto.CPF,
                    Endereco = dto.Endereco,
                    Profissao = dto.Profissao,
                    Empregador = dto.Empregador,
                    RendimentoMensal = dto.RendimentoMensal,
                    Permissao = usuario.Tipo
                };
            }
            else
            {
                var clienteEncontrado = await ObterPelasCredenciais(cpf);
                cliente = new ClienteDocumento()
                {
                    Id = clienteEncontrado.Id,
                    ClienteId = clienteEncontrado.ClienteId,
                    Nome = dto.Nome,
                    RG = dto.RG,
                    CPF = dto.CPF,
                    Endereco = dto.Endereco,
                    Profissao = dto.Profissao,
                    Empregador = dto.Empregador,
                    RendimentoMensal = dto.RendimentoMensal,
                    Permissao = clienteEncontrado.Permissao
                };
            }
            var resposta = await _containerCliente.ReplaceItemAsync(cliente, cliente.Id);
            return resposta.Resource;
        }
        catch
        {
            throw new Exception("Erro ao atualizar os dados de um cliente");
        }
    }

    public async Task<ClienteDocumento> Apagar(string credencial)
    {
        try
        {
            var id = await ObterId(credencial);
            var resposta = await _containerCliente.DeleteItemAsync<ClienteDocumento>(id, new PartitionKey(id));
            return resposta.Resource;
        }
        catch (Exception e)
        {
            throw new Exception("Houve um erro ao apagar um cliente.");
        }
    }

    public async Task<string> ObterId(string credencial)
    {
        try
        {
            var consulta = _containerCliente.GetItemLinqQueryable<ClienteDocumento>()
                .Where(c =>
                    c.CPF.Equals(credencial) ||
                    c.RG.ToLower().Equals(credencial.ToLower()))
                .ToFeedIterator();
            var clientes = new List<ClienteDocumento>();

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
            throw new Exception($"Erro ao consultar cliente pelo ID.");
        }
    }
}
