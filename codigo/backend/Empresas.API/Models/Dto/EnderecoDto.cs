using Newtonsoft.Json;

namespace Empresas.API.Models.Dto;

public class EnderecoDto
{
    [JsonProperty("rua")]
    public string Rua { get; set; } = string.Empty;

    [JsonProperty("numero")]
    public long Numero { get; set; }

    [JsonProperty("bairro")]
    public string Bairro { get; set; } = string.Empty;

    [JsonProperty("cidade")]
    public string Cidade { get; set; } = string.Empty;

    [JsonProperty("estado")]
    public string Estado { get; set; } = string.Empty;
}
