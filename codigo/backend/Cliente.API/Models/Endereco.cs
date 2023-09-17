using System.ComponentModel.DataAnnotations;

namespace Cliente.API.Models;

public class Endereco
{
    [Required]
    [MinLength(3), MaxLength(200)]
    public string Rua { get; set; } = string.Empty;

    [Required]
    [MinLength(1), MaxLength(6)]
    public int Numero { get; set; }

    [Required]
    [MinLength(2), MaxLength(50)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [MinLength(2), MaxLength(50)]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [MinLength(2), MaxLength(50)]
    public string Estado { get; set; } = string.Empty;

    [Key]
    [Required]
    [MinLength(8), MaxLength(8)]
    public string CEP { get; set; } = string.Empty;

}
