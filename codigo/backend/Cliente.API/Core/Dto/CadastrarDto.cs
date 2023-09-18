using System.ComponentModel.DataAnnotations;

namespace Cliente.API.Core.Dto;

public class CadastrarDto
{
    public string Nome { get; set; } = string.Empty;
    public string RG { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Profissao { get; set; } = string.Empty;
    public string Empregador { get; set; } = string.Empty;
    public decimal RendimentoMensal { get; set; }
    public string CEP { get; set; } = string.Empty;
    public string Rua { get; set; } = string.Empty;
    public int NumeroEndereco { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
