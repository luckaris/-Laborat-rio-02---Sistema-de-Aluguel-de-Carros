using System.ComponentModel.DataAnnotations;

namespace Auth.API.Core.Dtos;

public class AtualizarPermissoesDto
{
    [Required(ErrorMessage = "Nome de usuário é obrigatório.")]
    public string NomeDeUsuario { get; set; } = string.Empty;
}
