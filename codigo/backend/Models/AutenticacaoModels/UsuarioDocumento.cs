using Newtonsoft.Json;

namespace Models.AutenticacaoModels;

/// <summary>
/// Representa o documento que será armazenado no banco de dados Cosmos, as propriedades escritas
/// nessa classe são as que de fato serão realmente armazenadas no banco de dados, é a classe que
/// tem contato diretamente com o banco de dados.
/// </summary>
public class UsuarioDocumento
{
    [JsonProperty("id")]
    public required string Id { get; set; }

    [JsonProperty("usuarioId")]
    public required string UsuarioId { get; set; }

    [JsonProperty("nome")]
    public required string Nome { get; set; }

    [JsonProperty("identificador")]
    public required string Identificador { get; set; }

    [JsonProperty("senha")]
    public required string Senha { get; set; }

    [JsonProperty("tipo")]
    public required string Tipo { get; set; }
}