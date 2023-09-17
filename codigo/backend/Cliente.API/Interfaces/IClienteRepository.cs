using Cliente.API.Models;

namespace Cliente.API.Interfaces;

public interface IClienteRepository
{
    Task<ClienteModel> GetAll();
    Task<ClienteModel> GetByCpf(string cpf);
    Task<bool> Create(ClienteModel model);
    Task<ClienteModel> Update(string cpf, ClienteModel cliente);
    Task<ClienteModel> Delete(string cpf);
}
