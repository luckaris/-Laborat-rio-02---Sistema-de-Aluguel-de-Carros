using Newtonsoft.Json;

namespace Automoveis.API.Core.Dto
{
    public class PesquisarDto
    {
        [JsonProperty]
        public string modelo { get; set; } = string.Empty;
    }
}
