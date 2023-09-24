using Newtonsoft.Json;

namespace Models.ClientesModels.Dto;

public class AtualizarClienteDto
{
    [JsonProperty("nome")]
    public required string Nome { get; set; }

    [JsonProperty("rg")]
    public required string RG { get; set; }

    [JsonProperty("cpf")]
    public required string CPF { get; set; }

    [JsonProperty("endereco")]
    public string? Endereco { get; set; }

    [JsonProperty("profissao")]
    public required string Profissao { get; set; }

    [JsonProperty("empregador")]
    public required string Empregador { get; set; }

    [JsonProperty("rendimentoMensal")]
    public decimal RendimentoMensal { get; set; }
}
