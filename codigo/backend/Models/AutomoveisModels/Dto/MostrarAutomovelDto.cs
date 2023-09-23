using Newtonsoft.Json;

namespace Models.AutomoveisModels.Dto;

public class MostrarAutomovelDto
{
    [JsonProperty("ano")]
    public int Ano { get; set; }

    [JsonProperty("marca")]
    public string Marca { get; set; } = string.Empty;

    [JsonProperty("modelo")]
    public string Modelo { get; set; } = string.Empty;

    [JsonProperty("placa")]
    public string Placa { get; set; } = string.Empty;

    [JsonProperty("mensalidade")]
    public decimal Mensalidade { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}
