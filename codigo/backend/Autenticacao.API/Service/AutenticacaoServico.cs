using Autenticacao.API.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Models.AutenticacaoModels;
using Models.AutenticacaoModels.Dtos;
using System.Text.Json;

namespace Autenticacao.API.Service;

public class AutenticacaoServico : IAutenticacaoRepositorio
{
    private readonly IConfiguration _configuracao;
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;
    /// <summary>
    /// O construtor dessa classe realiza a Injeção de Dependência da classe que implementa da Interface na 
    /// qual a mesma está sendo passada por parâmetro, essa configuração é definina da classe Program, onde
    /// chamamos o método AddScoped.
    /// <br></br>>
    /// A mesma configura qual Container do Cosmos iremos usar para esse serviço.
    /// </summary>
    /// <param name="cosmosClient">Classe do CosmosClient na qual acessaremos o banco de dados.</param>
    /// <param name="configuration">Interface para obter dados do appsettings.</param>
    public AutenticacaoServico(CosmosClient cosmosClient, IConfiguration configuration)
    {
        _cosmosClient = cosmosClient;
        _configuracao = configuration;

        string nomeDoBancoDeDados = _configuracao["AzureCosmosDbSettings:DatabaseName"]!;
        var nomeDoContainer = "Usuarios";

        _container = _cosmosClient.GetContainer(nomeDoBancoDeDados, nomeDoContainer);
    }

    public async Task<UsuarioDocumento> Criar(CadastrarUsuarioDto dto)
    {
        try
        {
            var usuarioBuscado = await BuscarPeloIdentificador(dto.Identificador);
            if (usuarioBuscado != null) return null!;

            string guid = Guid.NewGuid().ToString();
            var usuario = new UsuarioDocumento()
            {
                Id = guid,
                UsuarioId = guid,
                Nome = dto.Nome,
                Identificador = dto.Identificador,
                Senha = dto.Senha,
                Tipo = dto.Tipo,
            };
            var response = await _container.CreateItemAsync(usuario);
            return response.Resource;
        }
        catch
        {
            var error = new
            {
                message = "Houve um erro durante o cadastro do usuário"
            };
            throw new Exception(JsonSerializer.Serialize(error));
        }
    }

    public async Task<UsuarioDocumento> BuscarPelasCredenciais(LogarUsuarioDto dto)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
                .Where(c => c.Identificador.Equals(dto.Identificador) && c.Senha.Equals(dto.Senha))
                .ToFeedIterator();

            var usuarios = new List<UsuarioDocumento>();
            while (consulta.HasMoreResults)
            {
                var response = await consulta.ReadNextAsync();
                usuarios.AddRange(response);
            }
            while (consulta.HasMoreResults)
            {
                var response = await consulta.ReadNextAsync();
                usuarios.AddRange(response);
            }
            if (usuarios.Count <= 0) return null!;
            return usuarios.FirstOrDefault()!;
        }
        catch
        {
            throw new Exception($"Erro ao buscar usuário, confira suas credenciais de acesso e tente novamente.");
        }
    }

    /// <summary>
    /// Método privado que busca um usuário pelo seu identificador único.
    /// </summary>
    /// <param name="identificador">Identificador único do usupario.</param>
    /// <returns>UsuarioDocumento</returns>
    /// <exception cref="Exception"></exception>
    private async Task<UsuarioDocumento> BuscarPeloIdentificador(string identificador)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
                .Where(c => c.Identificador.Equals(identificador)).ToFeedIterator();
            var usuarios = new List<UsuarioDocumento>();
            while (consulta.HasMoreResults)
            {
                var response = await consulta.ReadNextAsync();
                usuarios.AddRange(response);
            }
            if (usuarios.Count <= 0) return null!;
            return usuarios.FirstOrDefault()!;
        }
        catch
        {
            throw new Exception("Erro ao buscar usuário, confira suas credenciais de acesso e tente novamente.");
        }
    }
}
