using Newtonsoft.Json;

namespace Automoveis.API.Models
{
    public class AutomovelDocumento
    {
        [JsonProperty("renavam")]
        public string Renavam { get; set; } = string.Empty;

        [JsonProperty("ano")]
        public int Ano { get; set; }

        [JsonProperty("marca")]
        public string marca { get; set; } = string.Empty;

        [JsonProperty("modelo")]
        public string modelo { get; set; } = string.Empty;

        [JsonProperty("placa")]
        public string placa { get; set;} = string.Empty;

        [JsonProperty("mensalidade")]
        public decimal mensalidade { get; set; }

    }
}
