using System.ComponentModel.DataAnnotations;

namespace Cliente.API.Models;

public class ClienteModel 
{
    [Required]
    [MinLength(3), MaxLength(255)]
    public string Nome { get; private set; } = string.Empty;

    [Required]
    [MinLength(10), MaxLength(10)]
    public string Rg { get; private set; } = string.Empty;

    [Key]
    [Required]
    [MinLength(11), MaxLength(11)]
    public string Cpf { get; private set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Profissao { get; private set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Empregador { get; private set; } = string.Empty;

    [Required]
    public string EnderecoCEP { get; set; } = string.Empty;

    [Required]
    public required decimal RendimentoMensal { get; set; }

    public ClienteModel()
    {
        
    }
}
