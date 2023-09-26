using Newtonsoft.Json;

namespace Models.PedidosModels;

public class PedidoDocumento
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("pedido")]
    public string PedidoId { get; set; } = string.Empty;

    [JsonProperty("cpfCliente")]
    public string CpfCliente { get; set; } = string.Empty;

    [JsonProperty("placaAutomovel")]
    public string PlacaAutomovel { get; set; } = string.Empty;

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}
