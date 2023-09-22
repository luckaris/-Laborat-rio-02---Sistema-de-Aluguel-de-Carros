using Newtonsoft.Json;

namespace Empresas.API.Models.Dto;

public class BancoDto
{
    [JsonProperty("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonProperty("cnpj")]
    public string CNPJ { get; set; } = string.Empty;
}
