﻿using Newtonsoft.Json;

namespace Clientes.API.Models;

public class Endereco
{
    [JsonProperty("rua")]
    public string Rua { get; set; } = string.Empty;

    [JsonProperty("numero")]
    public long Numero { get; set; }

    [JsonProperty("bairro")]
    public string Bairro { get; set; } = string.Empty;

    [JsonProperty("cidade")]
    public string Cidade { get; set; } = string.Empty;

    [JsonProperty("estado")]
    public string Estado { get; set; } = string.Empty;
}