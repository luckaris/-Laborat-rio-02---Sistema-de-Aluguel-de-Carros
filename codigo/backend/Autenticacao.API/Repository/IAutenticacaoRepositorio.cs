using Autenticacao.API.Core.Dto;
using Autenticacao.API.Models;
using System.Security.Claims;

namespace Autenticacao.API.Repository;

public interface IAutenticacaoRepositorio
{
    Task<UsuarioDocumento> Criar(CadastrarDto dadosUsuario);
    Task<UsuarioDocumento> BuscarPelasCredenciais(LogarDto dto);
    Task<UsuarioDocumento> BuscarPeloCPF(string cpf);
    string GerarNovoJwt(List<Claim> claims);
}
