using Microsoft.IdentityModel.Tokens;
using Models.AutenticacaoModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Autenticacao.API.Service;

public static class TokenService
{
    /// <summary>
    /// Gera um token de autenticação para o usuário cadastrado ou logado.
    /// </summary>
    /// <param name="usuario">Representa a entidade usuuário.</param>
    /// <param name="chaveAppSettings">Chave privada obitda do appsettings.</param>
    /// <returns>string</returns>
    public static string GerarNovoJwt(UsuarioDocumento usuario, string chaveAppSettings)
    {
        byte[] chave = Encoding.UTF8.GetBytes(chaveAppSettings);
        var chaveSecreta = new SymmetricSecurityKey(chave);
        var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim(ClaimTypes.Role, usuario.Tipo),
        new Claim(ClaimTypes.NameIdentifier, usuario.Identificador)
        };

        var objetoDeToken = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(8),
            claims: claims,
            signingCredentials: credenciais);
        string token = new JwtSecurityTokenHandler().WriteToken(objetoDeToken);
        return token;
    }
}
