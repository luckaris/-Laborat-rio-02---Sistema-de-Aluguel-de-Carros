using Newtonsoft.Json;

namespace Models.AutenticacaoModels.Dtos;

public class LogarDto
{
    [JsonProperty("identificador")]
    public required string Identificador { get; set; }

    [JsonProperty("senha")]
    public required string Senha { get; set; }
}
