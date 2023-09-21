using Automoveis.API.Core.Dto;
using Newtonsoft.Json;

namespace Automoveis.API.Models
{
    public class AutomovelDocumento
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty ("automovelId")]
        public string AutomovelId { get; set;} = string.Empty;

        [JsonProperty("renavam")]
        public string Renavam { get; set; } = string.Empty;

        [JsonProperty("ano")]
        public int Ano { get; set; }

        [JsonProperty("marca")]
        public string Marca { get; set; } = string.Empty;

        [JsonProperty("modelo")]
        public string Modelo { get; set; } = string.Empty;

        [JsonProperty("placa")]
        public string Placa { get; set;} = string.Empty;

        [JsonProperty("mensalidade")]
        public decimal Mensalidade { get; set; }

        public AutomovelDocumento()
        {
            string guid = Guid.NewGuid().ToString();
            Id = guid;
            AutomovelId = guid;
        }

        public AutomovelDocumento(string guidParametro = "")
        {
            if (string.IsNullOrEmpty(guidParametro))
            {
                string guid = Guid.NewGuid().ToString();
                Id = guid;
                AutomovelId = guid;
            }
            else
            {
                Id = guidParametro;
                AutomovelId = guidParametro;
            }
        }

        public AutomovelDocumento(CadastrarDto dto)
        {
            string guid = Guid.NewGuid().ToString();
            Id = guid;
            AutomovelId = guid;
            Renavam = dto.Renavam;
            Ano = dto.Ano;
            Marca = dto.Marca;
            Modelo = dto.Modelo;
            Placa = dto.Placa;
            Mensalidade = dto.Mensalidade;
        }
    }
}
