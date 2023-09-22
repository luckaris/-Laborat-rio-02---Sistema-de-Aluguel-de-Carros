using Empresas.API.Models.Dto;
using Newtonsoft.Json;

namespace Empresas.API.Models;

public class EmpresaDocumento
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("empresaId")]
    public string EmpresaId { get; set; } = string.Empty;

    [JsonProperty("cnpj")]
    public string CNPJ { get; set; } = string.Empty;

    [JsonProperty("clientes")]
    public List<ClienteDto> Clientes { get; set; } = new List<ClienteDto>();

    [JsonProperty("bancos")]
    public List<BancoDto> Bancos { get; set; } = new List<BancoDto>();
}
