using Cliente.API.Models;

namespace Cliente.API.Interfaces;

public interface IClienteRepository
{
    Task<ClienteModel> GetAll();
    Task<ClienteModel> GetById(int id);
    Task<ClienteModel> GetByEmail(string email);
    Task<ClienteModel> Create(ClienteModel model);
    Task<ClienteModel> Update(string email, ClienteModel cliente);
    Task<ClienteModel> Delete(string email);
}
