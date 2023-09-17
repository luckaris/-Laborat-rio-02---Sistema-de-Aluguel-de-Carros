using Cliente.API.Interfaces;
using Cliente.API.Models;

namespace Cliente.API.Services;

public class ClienteService : IClienteRepository
{
    public Task<ClienteModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<ClienteModel> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ClienteModel> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<ClienteModel> Create(ClienteModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ClienteModel> Update(string email, ClienteModel cliente)
    {
        throw new NotImplementedException();
    }

    public Task<ClienteModel> Delete(string email)
    {
        throw new NotImplementedException();
    }
}
