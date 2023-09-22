using Autenticacao.API.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace Autenticacao.API.Dto;

public class CadastrarDto
{
    [JsonProperty("nome")]
    public required string Nome { get; set; }

    [JsonProperty("cpf")]
    public required string CPF { get; set; }

    [JsonProperty("senha")]
    public required string Senha { get; set; }
}
