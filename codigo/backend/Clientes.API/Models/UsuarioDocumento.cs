using Clientes.API.Core.Dto;
using Newtonsoft.Json;

namespace Clientes.API.Models;

public class UsuarioDocumento
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("usuarioId")]
    public string UsuarioId { get; set; } = string.Empty;

    [JsonProperty("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonProperty("rg")]
    public string RG { get; set; } = string.Empty;

    [JsonProperty("cpf")]
    public string CPF { get; set; } = string.Empty;

    [JsonProperty("senha")]
    public string Senha { get; set; } = string.Empty;

    [JsonProperty("endereco")]
    public Endereco? Endereco { get; set; }

    [JsonProperty("profissao")]
    public string Profissao { get; set; } = string.Empty;

    [JsonProperty("empregador")]
    public string Empregador { get; set; } = string.Empty;

    [JsonProperty("rendimentoMensal")]
    public decimal RendimentoMensal { get; set; }

    [JsonProperty("permissao")]
    public string Permissao { get; set; } = string.Empty;

    public UsuarioDocumento()
    {
        string guid = Guid.NewGuid().ToString();
        Id = guid;
        UsuarioId = guid;
        Permissao = EPermissaoAcesso.CLIENTE.ToString();
    }

    public UsuarioDocumento(string guidParametro = "")
    {
        if(string.IsNullOrEmpty(guidParametro))
        {
            string guid = Guid.NewGuid().ToString();
            Id = guid;
            UsuarioId = guid;
        } else
        {
            Id = guidParametro;
            UsuarioId = guidParametro;
        }

        Permissao = EPermissaoAcesso.CLIENTE.ToString();
    }

    public UsuarioDocumento(CadastrarDto dto)
    {
        string guid = Guid.NewGuid().ToString();
        Id = guid;
        UsuarioId = guid;
        Nome = dto.Nome;
        RG = dto.RG;
        CPF = dto.CPF;
        Senha = dto.Senha;
        Endereco = dto.Endereco;
        Profissao = dto.Profissao;
        Empregador = dto.Empregador;
        RendimentoMensal = dto.RendimentoMensal;
        Permissao = EPermissaoAcesso.CLIENTE.ToString();
    }
}
