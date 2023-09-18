using Cliente.API.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace Cliente.API.Models;

public class EnderecoEntidade
{
    #region Propriedades da classe
    [Key]
    [Required]
    [MinLength(8), MaxLength(8)]
    public string CEP { get; set; } = string.Empty;

    [Required]
    [MinLength(11), MaxLength(11)]
    public string ClienteCPF { get; set; } = string.Empty;

    [Required]
    [MinLength(3), MaxLength(200)]
    public string Rua { get; set; } = string.Empty;

    [Required]
    [MinLength(1), MaxLength(6)]
    public int Numero { get; set; }

    [Required]
    [MinLength(1), MaxLength(50)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [MinLength(1), MaxLength(50)]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [MinLength(2), MaxLength(50)]
    public string Estado { get; set; } = string.Empty;
    #endregion Propriedades da classe

    #region Construtores da classe
    public EnderecoEntidade()
    {
        
    }

    public EnderecoEntidade(CadastrarDto dto)
    {
        CEP = dto.CEP;
        Rua = dto.Rua;
        Numero = dto.NumeroEndereco;
        Bairro = dto.Bairro;
        Cidade = dto.Cidade;
        Estado = dto.Estado;
        ClienteCPF = dto.CPF;
    }

    public EnderecoEntidade(AtualizarDto dto)
    {
        CEP = dto.CEP;
        Rua = dto.Rua;
        Numero = dto.NumeroEndereco;
        Bairro = dto.Bairro;
        Cidade = dto.Cidade;
        Estado = dto.Estado;
        ClienteCPF = dto.CPF;
    }
    #endregion Construtores da classe
}
