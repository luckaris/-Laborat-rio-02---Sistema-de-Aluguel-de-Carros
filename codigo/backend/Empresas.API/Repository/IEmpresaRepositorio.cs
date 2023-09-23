using Empresas.API.Models;
using Empresas.API.Models.Dto;

namespace Empresas.API.Repository;

public interface IEmpresaRepositorio
{
    Task<List<EmpresaDocumento>> ObterTodos(int paginacao);
    Task<EmpresaDocumento> ObterPelasCredenciais(string credencial);
    Task<EmpresaDocumento> Cadastrar(CadastrarDto empresa);
    Task<EmpresaDocumento> Atualizar(string credencial, AtualizarDto documento);
    Task<EmpresaDocumento> Apagar(string credencial);
}
