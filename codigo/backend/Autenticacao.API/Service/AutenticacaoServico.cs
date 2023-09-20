using Autenticacao.API.Core.Dto;
using Autenticacao.API.Models;
using Autenticacao.API.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Autenticacao.API.Service;

public class AutenticacaoServico : IAutenticacaoRepositorio
{
    private readonly CosmosClient _cosmosClient;
    private readonly IConfiguration _configuracao;
    private readonly Container _container;
    public AutenticacaoServico(CosmosClient cosmosClient, IConfiguration configuration)
    {
        _cosmosClient = cosmosClient;
        _configuracao = configuration;

        string nomeDoBancoDeDados = _configuracao["AzureCosmosDbSettings:DatabaseName"]!;
        var nomeDoContainer = "Usuarios";

        _container = _cosmosClient.GetContainer(nomeDoBancoDeDados, nomeDoContainer);
    }

    public async Task<UsuarioDocumento> Criar(CadastrarDto dadosUsuario)
    {
        try
        {
            var usuarioBuscado = await BuscarPeloCPF(dadosUsuario.CPF);
            if(usuarioBuscado != null) return null!;
            
            var usuario = new UsuarioDocumento()
            {
                Nome = dadosUsuario.Nome,
                RG = "",
                CPF = dadosUsuario.CPF,
                Senha = dadosUsuario.Senha,
                Endereco = new Endereco(),
                Profissao = "",
                Empregador = "",
                RendimentoMensal = -1
            };
            var response = await _container.CreateItemAsync(usuario);
            return response.Resource;
        }
        catch
        {
            throw new Exception("Erro ao cadastrar um usuário no banco de dados");
        }
    }

    public async Task<UsuarioDocumento> BuscarPelasCredenciais(LogarDto dto)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
                .Where(c => c.CPF.Equals(dto.CPF) && c.Senha.Equals(dto.Senha))
                .ToFeedIterator();

            var usuarios = new List<UsuarioDocumento>();
            while (consulta.HasMoreResults)
            {
                var response = await consulta.ReadNextAsync();
                usuarios.AddRange(response);
            }

            return usuarios.FirstOrDefault()!;
        }
        catch
        {
            throw new Exception($"Erro ao buscar usuário, confira suas credenciais de acesso e tente novamente.");
        }
    }

    public async Task<UsuarioDocumento> BuscarPeloCPF(string cpf)
    {
        try
        {
            var consulta = _container.GetItemLinqQueryable<UsuarioDocumento>()
                .Where(c => c.CPF.Equals(cpf))
                .ToFeedIterator();

            var usuarios = new List<UsuarioDocumento>();
            while (consulta.HasMoreResults)
            {
                var response = await consulta.ReadNextAsync();
                usuarios.AddRange(response);
            }

            return usuarios.FirstOrDefault()!;
        }
        catch
        {
            throw new Exception($"Erro ao buscar usuário, confira suas credenciais de acesso e tente novamente.");
        }
    }

    public string GerarNovoJwt(List<Claim> claims)
    {
        string secreta = _configuracao["Jwt:ChaveSecreta"]!;
        byte[] chave = Encoding.UTF8.GetBytes(secreta);

        var chaveSecreta = new SymmetricSecurityKey(chave);
        var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

        var objetoDeToken = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(8),
            claims: claims,
            signingCredentials: credenciais);
        string token = new JwtSecurityTokenHandler().WriteToken(objetoDeToken);
        return token;
    }
}
