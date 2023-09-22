using Newtonsoft.Json;

namespace Automoveis.API.Dto;

public class PesquisarDto
{
    [JsonProperty("modelo")]
    public string Modelo { get; set; } = string.Empty;
}
