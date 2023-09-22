using Autenticacao.API.Models;
using Newtonsoft.Json;

namespace Autenticacao.API.Dto;

public class LogarDto
{
    [JsonProperty("cpf")]
    public required string CPF { get; set; }

    [JsonProperty("senha")]
    public required string Senha { get; set; }
}
