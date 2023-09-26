using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Models.PedidosModels;
using Models.PedidosModels.Dto;
using Pedidos.API.Repository;

namespace Pedidos.API.Services;

public class PedidoServico : IPedidoRepositorio
{
    private readonly CosmosClient _cosmosClient;
    private readonly IConfiguration _configuracao;
    private readonly Container _containerPedido;
    public PedidoServico(CosmosClient cosmosClient, IConfiguration configuracao)
    {
        _cosmosClient = cosmosClient;
        _configuracao = configuracao;

        string nomeDoBancoDeDados = _configuracao["AzureCosmosDbSettings:DatabaseName"]!;
        var nomeDoContainer = "Pedidos";
        _containerPedido = _cosmosClient.GetContainer(nomeDoBancoDeDados, nomeDoContainer);
    }
    public async Task<List<MostrarPedidoDto>> ObterTodos(int paginacao)
    {
        try
        {
            QueryDefinition consulta;

            if (paginacao == 0)
                consulta = _containerPedido.GetItemLinqQueryable<MostrarPedidoDto>()
                    .ToQueryDefinition();
            else
                consulta = _containerPedido.GetItemLinqQueryable<MostrarPedidoDto>()
                    .Take(paginacao).ToQueryDefinition();

            var response = await _containerPedido.GetItemQueryIterator<MostrarPedidoDto>(consulta).ReadNextAsync();
            return response.ToList();
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao consultar todos os pedidos.");
        }
    }

    public async Task<MostrarPedidoDto> ObterPelaPlaca(string placa)
    {
        try
        {
            var consulta = _containerPedido.GetItemLinqQueryable<PedidoDocumento>()
            .Where(c => c.CpfCliente.Equals(placa)).ToFeedIterator();

            var pedidos = new List<PedidoDocumento>();
            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                pedidos.AddRange(resposta);
            }
            return new MostrarPedidoDto()
            {
                CpfCliente = pedidos.FirstOrDefault()!.CpfCliente,
                PlacaAutomovel = pedidos.FirstOrDefault()!.PlacaAutomovel,
                Status = pedidos.FirstOrDefault()!.Status
            };
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao consultar pedido em específico.");
        }
    }

    public async Task<List<MostrarPedidoDto>> ObterPeloStatus(string status)
    {
        try
        {
            var consulta = _containerPedido.GetItemLinqQueryable<PedidoDocumento>()
                .Where(c => c.Status.Equals(status)).ToFeedIterator();
            var pedidos = new List<PedidoDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                pedidos.AddRange(resposta);
            }

            return GerarListaDePedidosNaTela(pedidos);
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao consultar pedidos pelo Status.");
        }
    }

    public async Task<MostrarPedidoDto> Criar(CadastrarPedidoDto dto)
    {
        try
        {
            var pedido = await ObterPelaPlaca(dto.CpfCliente);
            if (pedido != null) return null!;

            string guid = Guid.NewGuid().ToString();
            var pedidoDocumento = new PedidoDocumento()
            {
                Id = guid,
                PedidoId = guid,
                CpfCliente = dto.CpfCliente,
                PlacaAutomovel = dto.PlacaAutomovel,
                Status = dto.Status
            };

            var resposta = await _containerPedido.CreateItemAsync(pedidoDocumento);
            return new MostrarPedidoDto()
            {
                CpfCliente = resposta.Resource.CpfCliente,
                PlacaAutomovel = resposta.Resource.PlacaAutomovel,
                Status = resposta.Resource.Status
            };
        }
        catch (Exception)
        {
            throw new Exception($"Erro ao cadastrar um cliente.");
        }
    }

    public async Task<MostrarPedidoDto> Atualizar(string cpf, AtualizarPedidoDto dto)
    {
        try
        {
            var id = await ObterId(cpf, dto.PlacaAutomovel);
            if (string.IsNullOrEmpty(id)) return null!;
            var pedido = new PedidoDocumento()
            {
                Id = id,
                PedidoId = id,
                CpfCliente = dto.CpfCliente,
                PlacaAutomovel = dto.PlacaAutomovel,
                Status = dto.Status
            };

            var resposta = await _containerPedido.ReplaceItemAsync(pedido, id);
            return new MostrarPedidoDto
            {
                CpfCliente = dto.CpfCliente,
                PlacaAutomovel = dto.PlacaAutomovel,
                Status = dto.Status
            };
        }
        catch
        {
            throw new Exception("Erro ao atualizar os dados de um cliente");
        }
    }

    public async Task<MostrarPedidoDto> Apagar(string cpf, string placa)
    {
        try
        {
            var id = await ObterId(cpf, placa);
            var resposta = await _containerPedido.DeleteItemAsync<PedidoDocumento>(id, new PartitionKey(id));
            if (resposta is null) return null!;
            return new MostrarPedidoDto
            {
                CpfCliente = resposta.Resource.CpfCliente,
                PlacaAutomovel = resposta.Resource.PlacaAutomovel,
                Status = resposta.Resource.Status
            };
        }
        catch
        {
            throw new Exception("Houve um erro ao apagar um cliente.");
        }
    }

    private List<MostrarPedidoDto> GerarListaDePedidosNaTela(List<PedidoDocumento> pedidos)
    {
        List<MostrarPedidoDto> dtos = new();
        foreach (var documento in pedidos)
        {
            dtos.Add(new MostrarPedidoDto()
            {
                CpfCliente = documento.CpfCliente,
                PlacaAutomovel = documento.PlacaAutomovel,
                Status = documento.Status
            });
        }
        return dtos;
    }

    private async Task<string> ObterId(string cpf, string placa)
    {
        try
        {
            var consulta = _containerPedido.GetItemLinqQueryable<PedidoDocumento>()
            .Where(c => c.CpfCliente.Equals(cpf) && c.PlacaAutomovel.Equals(placa)).ToFeedIterator();
            var pedidos = new List<PedidoDocumento>();

            while (consulta.HasMoreResults)
            {
                var resposta = await consulta.ReadNextAsync();
                pedidos.AddRange(resposta);
            }
            return pedidos.FirstOrDefault()!.Id;
        }
        catch (Exception)
        {
            throw new Exception("Houve um erro ao consultar pedido em específico.");
        }
    }
}
