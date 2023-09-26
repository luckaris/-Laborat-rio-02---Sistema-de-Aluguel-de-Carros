using Newtonsoft.Json;

namespace Models.PedidosModels.Dto;

public class CadastrarPedidoDto
{
    [JsonProperty("cpfCliente")]
    public string CpfCliente { get; set; } = string.Empty;

    [JsonProperty("placaAutomovel")]
    public string PlacaAutomovel { get; set; } = string.Empty;

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}
