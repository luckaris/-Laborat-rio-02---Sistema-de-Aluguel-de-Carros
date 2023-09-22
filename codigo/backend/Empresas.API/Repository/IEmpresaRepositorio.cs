using Empresas.API.Models;
using Empresas.API.Models.Dto;

namespace Empresas.API.Repository;

public interface IEmpresaRepositorio
{
    Task<List<EmpresaDocumento>> ObterTodos(int paginacao);
    Task<EmpresaDocumento> ObterPorId(string id);
    Task<EmpresaDocumento> ObterPorEmpresaId(string empresaId);
    Task<EmpresaDocumento> ObterPeloCNPJ(string cnpj);
    Task<EmpresaDocumento> Cadastrar(CadastrarDto empresa);
    Task<EmpresaDocumento> Atualizar(string cnpj, AtualizarDto documento);
    Task<EmpresaDocumento> Apagar(string id, string empresaId);
}
