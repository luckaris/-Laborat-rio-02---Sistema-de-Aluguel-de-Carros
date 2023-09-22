using Newtonsoft.Json;

namespace Empresas.API.Models.Dto;

public class ClienteDto
{
    [JsonProperty("nome")]
    public required string Nome { get; set; }

    [JsonProperty("rg")]
    public required string RG { get; set; }

    [JsonProperty("cpf")]
    public required string CPF { get; set; }

    [JsonProperty("endereco")]
    public EnderecoDto? Endereco { get; set; } = new EnderecoDto();

    [JsonProperty("profissao")]
    public required string Profissao { get; set; }

    [JsonProperty("empregador")]
    public required string Empregador { get; set; }

    [JsonProperty("rendimentoMensal")]
    public decimal RendimentoMensal { get; set; }
}
