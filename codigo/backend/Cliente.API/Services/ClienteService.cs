using Cliente.API.Interfaces;
using Cliente.API.Models;

namespace Cliente.API.Services;

public class ClienteService : IClienteRepository
{
    public Task<ClienteModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<ClienteModel> GetByCpf(string cpf)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Create(ClienteModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ClienteModel> Update(string cpf, ClienteModel cliente)
    {
        throw new NotImplementedException();
    }

    public Task<ClienteModel> Delete(string cpf)
    {
        throw new NotImplementedException();
    }
}
