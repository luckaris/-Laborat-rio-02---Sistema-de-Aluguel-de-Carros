using Confluent.Kafka;
using Pedidos.API.Models;
using System.Text.Json;

namespace Pedidos.API.Service;

public class ProducerService
{
    private readonly IConfiguration _configuration;
    private readonly ProducerConfig _producerConfig;
    private readonly ILogger<ProducerService> _logger;
    public ProducerService(IConfiguration configuration, ILogger<ProducerService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        var boostrap = _configuration.GetSection("KafkaConfig")
            .GetSection("BootstrapServer").Value;
        _producerConfig = new ProducerConfig()
        {
            BootstrapServers = boostrap
        };
    }

    public async Task<string> SendMessage(string message)
    {
        var topic = _configuration.GetSection("KafkaConfig")
            .GetSection("TopicName").Value;
        try
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();

            var result = await producer.ProduceAsync(topic: topic, new() { Value = message });
            _logger.LogInformation(result.Status.ToString() + " - " + message);

            return result.Status.ToString() + " - " + message;
        }
        catch
        {
            _logger.LogError("Erro ao enviar mensagem");
            return "Erro ao enviar mensagem";
        }
    }

    public async Task<string> SendMessage(Pedido pessoa)
    {
        var topic = _configuration.GetSection("KafkaConfig")
            .GetSection("TopicName").Value;
        try
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
            var message = JsonSerializer.Serialize(pessoa);
            var result = await producer.ProduceAsync(topic: topic, new() { Value = message });
            _logger.LogInformation(result.Status.ToString() + " - " + message);

            return result.Status.ToString() + " - " + message;
        }
        catch
        {
            _logger.LogError("Erro ao enviar mensagem");
            return "Erro ao enviar mensagem";
        }
    }
}
