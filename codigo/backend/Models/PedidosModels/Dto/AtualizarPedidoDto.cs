using Newtonsoft.Json;

namespace Models.PedidosModels.Dto;

public class AtualizarPedidoDto
{
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}
