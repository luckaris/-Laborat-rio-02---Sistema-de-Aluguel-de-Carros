using Newtonsoft.Json;

namespace Autenticacao.API.Models;

public class UsuarioDocumento
{
    [JsonProperty("id")]
    public required string Id { get; set; }

    [JsonProperty("usuarioId")]
    public required string UsuarioId { get; set; }

    [JsonProperty("nome")]
    public required string Nome { get; set; }

    [JsonProperty("rg")]
    public required string RG { get; set; }

    [JsonProperty("cpf")]
    public required string CPF { get; set; }

    [JsonProperty("senha")]
    public required string Senha { get; set; }

    [JsonProperty("endereco")]
    public Endereco? Endereco { get; set; }

    [JsonProperty("profissao")]
    public required string Profissao { get; set; }

    [JsonProperty("empregador")]
    public required string Empregador { get; set; }

    [JsonProperty("rendimentoMensal")]
    public decimal RendimentoMensal { get; set; }

    [JsonProperty("permissao")]
    public required string Permissao { get; set; }
}
