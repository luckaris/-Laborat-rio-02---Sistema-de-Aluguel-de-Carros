﻿using Autenticacao.API.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace Autenticacao.API.Core.Dto;

public class CadastrarDto
{
    [JsonProperty("nome")]
    public required string Nome { get; set; }

    [JsonProperty("rg")]
    public required string RG { get; set; }

    [JsonProperty("cpf")]
    public required string CPF { get; set; }

    [JsonProperty("senha")]
    public required string Senha { get; set; }

    [JsonProperty("endereco")]
    public Endereco? Endereco { get; set; }

    [JsonProperty("profissao")]
    public required string Profissao { get; set; }

    [JsonProperty("empregador")]
    public required string Empregador { get; set; }

    [JsonProperty("rendimentoMensal")]
    public decimal RendimentoMensal { get; set; }
}
