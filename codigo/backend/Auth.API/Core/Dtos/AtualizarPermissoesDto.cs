using System.ComponentModel.DataAnnotations;

namespace Auth.API.Core.Dtos;

public class AtualizarPermissoesDto
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;
}
