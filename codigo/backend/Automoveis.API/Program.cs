using Automoveis.API.Repository;
using Automoveis.API.Services;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

var allowAll = "allowAll";
var linkEndpointUri = builder.Configuration["AzureCosmosDbSettings:EndpointUri"];
var chavePrimariaDeConexao = builder.Configuration["AzureCosmosDbSettings:PrimaryKey"];
var nomeDoBancoDeDados = builder.Configuration["AzureCosmosDbSettings:DatabaseName"];

builder.Services.AddCors(options => options.AddPolicy(name: allowAll, policy => { 
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); 
}));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(provider =>
{
    CosmosClientOptions cosmosClientOptions = new()
    {
        ApplicationName = nomeDoBancoDeDados
    };

    var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    CosmosClient cosmosClient = new(linkEndpointUri, chavePrimariaDeConexao, cosmosClientOptions);
    cosmosClient.ClientOptions.ConnectionMode = ConnectionMode.Gateway;

    return cosmosClient;
});
builder.Services.AddScoped<IAutomovelRepositorio, AutomovelServico>();

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
