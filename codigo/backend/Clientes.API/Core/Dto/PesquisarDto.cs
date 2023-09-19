using Newtonsoft.Json;

namespace Clientes.API.Core.Dto;

public class PesquisarDto
{
    [JsonProperty("nome")]
    public string Nome { get; set; } = string.Empty;
}
