using Cliente.API.Core.Context;
using Cliente.API.Interfaces;
using Cliente.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cliente.API.Services;

public class ClienteService : IClienteRepository
{
    private readonly AppDbContext _context;
    public ClienteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClienteModel>> GetAll()
    {
        try
        {
            var clientes = await _context.Clientes.ToListAsync();
            return clientes;
        } catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ClienteModel> GetByCpf(string cpf)
    {
        try
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);
            return cliente!;
        } catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> Create(ClienteModel model)
    {
        try
        {
            await _context.Clientes.AddAsync(model);
            await _context.SaveChangesAsync();

            var tentandoObterOCliente = await GetByCpf(model.Cpf);
            if (tentandoObterOCliente is null) return false;
            return true;
        } catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ClienteModel> Update(string cpf, ClienteModel cliente)
    {
        try
        {
            var clienteEncontrado = await GetByCpf(cpf);
            if (clienteEncontrado is null) return null;

            clienteEncontrado.Nome = cliente.Nome;
            clienteEncontrado.Profissao = cliente.Profissao;
            clienteEncontrado.Empregador = cliente.Empregador;
            clienteEncontrado.EnderecoCEP = cliente.EnderecoCEP;
            clienteEncontrado.RendimentoMensal = cliente.RendimentoMensal;

            await _context.SaveChangesAsync();

            return clienteEncontrado;
        } catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> Delete(string cpf)
    {
        try
        {
            var clienteEncontrado = await GetByCpf(cpf);
            if (clienteEncontrado is null) return false;
            _context.Remove(clienteEncontrado);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
