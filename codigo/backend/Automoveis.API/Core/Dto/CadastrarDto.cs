using Newtonsoft.Json;

namespace Automoveis.API.Core.Dto
{
    public class CadastrarDto
    {
        [JsonProperty("renavam")]
        public required string Renavam { get; set; }

        [JsonProperty("ano")]
        public required int Ano { get; set; }

        [JsonProperty("marca")]
        public required string Marca { get; set; }

        [JsonProperty("modelo")]
        public required string Modelo { get; set; }

        [JsonProperty("placa")]
        public required string Placa { get; set; }

        [JsonProperty("mensalidade")]
        public decimal Mensalidade { get; set; }
    }
}
