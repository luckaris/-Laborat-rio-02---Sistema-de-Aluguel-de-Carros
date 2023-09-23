using Newtonsoft.Json;

namespace Models.AutenticacaoModels.Dtos;

public class CadastrarUsuarioDto
{
    [JsonProperty("nome")]
    public required string Nome { get; set; }

    [JsonProperty("identificador")]
    public required string Identificador { get; set; }

    [JsonProperty("senha")]
    public required string Senha { get; set; }

    [JsonProperty("tipo")]
    public required string Tipo { get; set; }
}
