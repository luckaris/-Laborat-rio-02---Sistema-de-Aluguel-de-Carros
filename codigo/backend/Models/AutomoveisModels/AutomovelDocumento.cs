using Newtonsoft.Json;

namespace Models.AutomoveisModels;

public class AutomovelDocumento
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("automovelId")]
    public string AutomovelId { get; set; } = string.Empty;

    [JsonProperty("renavam")]
    public string Renavam { get; set; } = string.Empty;

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
