using Cliente.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cliente.API.Core.Context;

public class AppDbContext : DbContext
{
    public DbSet<ClienteEntidade> Clientes => Set<ClienteEntidade>();
    public DbSet<EnderecoEntidade> Enderecos => Set<EnderecoEntidade>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
