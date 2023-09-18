using Cliente.API.Core.Context;
using Cliente.API.Core.Dto;
using Cliente.API.Interfaces;
using Cliente.API.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace Cliente.API.Services;

public class ClienteService : IClienteRepositorio
{
    private readonly AppDbContext _context;
    public ClienteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClienteEntidade>> ObtendoTodos()
    {
        try
        {
            var clientes = await _context.Clientes.ToListAsync();
            return clientes;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ClienteEntidade> ObtendoPeloCPF(string cpf)
    {
        try
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.CPF == cpf);
            return cliente!;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }



    public async Task<bool> Criar(CadastrarDto dto)
    {
        var cliente = new ClienteEntidade(dto);
        var endereco = new EnderecoEntidade(dto);
        try
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();

            var tentandoObterOCliente = await ObtendoPeloCPF(cliente.CPF);
            var tentandoObterEndereco = await ObtendoEnderecoPeloCEP(cliente.EnderecoCEP);
            if (tentandoObterOCliente is null || tentandoObterEndereco is null) return false;
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ClienteEntidade> Atualizar(string cpf, AtualizarDto dto)
    {
        try
        {
            var clienteEncontrado = await ObtendoPeloCPF(cpf);
            var enderecoEncontrado = await ObtendoEnderecoPeloCPF(clienteEncontrado.CPF);

            if (clienteEncontrado is null) return null!;
            if (enderecoEncontrado is null) return null!;

            clienteEncontrado.RG = dto.RG;
            clienteEncontrado.CPF = dto.CPF;
            clienteEncontrado.Nome = dto.Nome;
            clienteEncontrado.Email = dto.Email;
            clienteEncontrado.Profissao = dto.Profissao;
            clienteEncontrado.Empregador = dto.Empregador;
            clienteEncontrado.RendimentoMensal = dto.RendimentoMensal;
            clienteEncontrado.EnderecoCEP = dto.CEP;

            enderecoEncontrado.CEP = dto.CEP;
            enderecoEncontrado.Rua = dto.Rua;
            enderecoEncontrado.Numero = dto.NumeroEndereco;
            enderecoEncontrado.Bairro = dto.Bairro;
            enderecoEncontrado.Cidade = dto.Cidade;
            enderecoEncontrado.Estado = dto.Estado;
            enderecoEncontrado.ClienteCPF = dto.CPF;

            await _context.SaveChangesAsync();
            return clienteEncontrado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> Remover(string cpf)
    {
        try
        {
            var clienteEncontrado = await ObtendoPeloCPF(cpf);
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

    public async Task<EnderecoEntidade> ObtendoEnderecoPeloCEP(string enderecoCEP)
    {
        try
        {
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.CEP == enderecoCEP);
            return endereco!;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EnderecoEntidade> ObtendoEnderecoPeloCPF(string cpf)
    {
        try
        {
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.ClienteCPF == cpf);
            return endereco!;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
