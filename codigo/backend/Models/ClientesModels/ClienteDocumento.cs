using Newtonsoft.Json;

namespace Models.ClientesModels;

public class ClienteDocumento
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("clienteId")]
    public string ClienteId { get; set; } = string.Empty;

    [JsonProperty("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonProperty("rg")]
    public string RG { get; set; } = string.Empty;

    [JsonProperty("cpf")]
    public string CPF { get; set; } = string.Empty;

    [JsonProperty("endereco")]
    public string? Endereco { get; set; }

    [JsonProperty("profissao")]
    public string Profissao { get; set; } = string.Empty;

    [JsonProperty("empregador")]
    public string Empregador { get; set; } = string.Empty;

    [JsonProperty("rendimentoMensal")]
    public decimal RendimentoMensal { get; set; }

    [JsonProperty("permissao")]
    public string Permissao { get; set; } = string.Empty;
}
