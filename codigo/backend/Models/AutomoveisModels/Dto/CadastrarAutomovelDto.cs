using Newtonsoft.Json;

namespace Models.AutomoveisModels.Dto;

public class CadastrarAutomovelDto
{
    [JsonProperty("renavam")]
    public required string Renavam { get; set; }

    [JsonProperty("ano")]
    public required int Ano { get; set; }

    [JsonProperty("marca")]
    public required string Marca { get; set; }

    [JsonProperty("modelo")]
    public required string Modelo { get; set; }

    [JsonProperty("placa")]
    public required string Placa { get; set; }

    [JsonProperty("mensalidade")]
    public required decimal Mensalidade { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}
