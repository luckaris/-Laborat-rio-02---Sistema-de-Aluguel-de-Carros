using Cliente.API.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace Cliente.API.Models;

public class ClienteEntidade
{
    #region Propriedades da classe
    [Required]
    [MaxLength(255)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MinLength(10), MaxLength(10)]
    public string RG { get; set; } = string.Empty;

    [Required]
    [Key]
    [MinLength(11), MaxLength(11)]
    public string CPF { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Profissao { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Empregador { get; set; } = string.Empty;

    [Required]
    [MinLength(8), MaxLength(8)]
    public string EnderecoCEP { get; set; } = string.Empty;

    [Required]
    public decimal RendimentoMensal { get; set; }
    #endregion Propriedades da classe

    #region Construtores da classe
    public ClienteEntidade()
    {

    }

    public ClienteEntidade(CadastrarDto dto)
    {
        RG = dto.RG;
        CPF = dto.CPF;
        Nome = dto.Nome;
        Email = dto.Email;
        Profissao = dto.Profissao;
        Empregador = dto.Empregador;
        RendimentoMensal = dto.RendimentoMensal;
        EnderecoCEP = dto.CEP;
    }

    public ClienteEntidade(AtualizarDto dto)
    {
        RG = dto.RG;
        CPF = dto.CPF;
        Nome = dto.Nome;
        Email = dto.Email;
        Profissao = dto.Profissao;
        Empregador = dto.Empregador;
        RendimentoMensal = dto.RendimentoMensal;
        EnderecoCEP = dto.CEP;
    }
    #endregion Construtores da classe
}
