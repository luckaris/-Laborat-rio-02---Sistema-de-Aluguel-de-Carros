using Autenticacao.API.Core.Dto;
using Newtonsoft.Json;

namespace Autenticacao.API.Models;

public class UsuarioDocumento
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("usuarioId")]
    public string UsuarioId { get; set; }

    [JsonProperty("nome")]
    public string Nome { get; set; }

    [JsonProperty("rg")]
    public string RG { get; set; }

    [JsonProperty("cpf")]
    public string CPF { get; set; }

    [JsonProperty("senha")]
    public string Senha { get; set; }

    [JsonProperty("endereco")]
    public Endereco? Endereco { get; set; }

    [JsonProperty("profissao")]
    public string Profissao { get; set; }

    [JsonProperty("empregador")]
    public string Empregador { get; set; }

    [JsonProperty("rendimentoMensal")]
    public decimal RendimentoMensal { get; set; }

    [JsonProperty("permissao")]
    public string Permissao { get; set; }

    public UsuarioDocumento()
    {
        string guid = Guid.NewGuid().ToString();
        Id = guid;
        UsuarioId = guid;
        Permissao = EPermissaoAcesso.CLIENTE.ToString();
    }

    public UsuarioDocumento(CadastrarDto dto)
    {
        string guid = Guid.NewGuid().ToString();
        Id = guid;
        UsuarioId = guid;
        Nome = dto.Nome;
        RG = "";
        CPF = dto.CPF;
        Senha = dto.Senha;
        Endereco = new Endereco();
        Profissao = "";
        Empregador = "";
        RendimentoMensal = -1;
        Permissao = EPermissaoAcesso.CLIENTE.ToString();
    }
}
