using Newtonsoft.Json;

namespace Empresas.API.Models.Dto;

public class CadastrarDto
{
    [JsonProperty("cnpj")]
    public string CNPJ { get; set; } = string.Empty;

    [JsonProperty("clientes")]
    public List<ClienteDto> Clientes { get; set; } = new List<ClienteDto>();

    [JsonProperty("bancos")]
    public List<BancoDto> Bancos { get; set; } = new List<BancoDto>();
}
