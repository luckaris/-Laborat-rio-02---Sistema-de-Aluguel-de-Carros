using Cliente.API.Core.Context;
using Cliente.API.Interfaces;
using Cliente.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var allowAll = "allowAll";

var connectionString = builder.Configuration.GetConnectionString("local");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddCors(options => options.AddPolicy(name: allowAll, policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IClienteRepositorio, ClienteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowAll);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
