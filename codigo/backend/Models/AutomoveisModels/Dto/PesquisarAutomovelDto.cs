using Newtonsoft.Json;

namespace Models.AutomoveisModels.Dto;

public class PesquisarAutomovelDto
{
    [JsonProperty("modelo")]
    public string Modelo { get; set; } = string.Empty;

    [JsonProperty("marca")]
    public string Marca { get; set; } = string.Empty;

    [JsonProperty("mensalidade")]
    public decimal Mensalidade { get; set; }

    [JsonProperty("anoInicial")]
    public int AnoInicial { get; set; }

    [JsonProperty("anoFinal")]
    public int AnoFinal { get; set; }

    [JsonProperty("status")]
    public List<string> Status { get; set; } = new();
}
