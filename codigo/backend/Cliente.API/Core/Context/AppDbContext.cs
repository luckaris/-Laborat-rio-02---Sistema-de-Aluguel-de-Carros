using Cliente.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cliente.API.Core.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<ClienteModel> Clientes => Set<ClienteModel>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();
}
