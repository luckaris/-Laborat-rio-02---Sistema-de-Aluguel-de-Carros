using Newtonsoft.Json;

namespace Models.ClientesModels.Dto;

public class PesquisarClienteDto
{
    [JsonProperty("nome")]
    public string Nome { get; set; } = string.Empty;
}
