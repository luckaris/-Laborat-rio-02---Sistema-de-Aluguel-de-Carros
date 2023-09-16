using System.ComponentModel.DataAnnotations;

namespace Auth.API.Core.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "Nome de usuário é obrigatório.")]
    public string NomeDeUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória.")]
    public string Senha { get; set; } = string.Empty;
}
