// VigiLant.Services/MqttService.cs
using MQTTnet;
using MQTTnet.Client;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text;

namespace VigiLant.Services
{
    public class MqttService : IMqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly ILogger<MqttService> _logger;

        public MqttService(IMqttClient mqttClient, ILogger<MqttService> logger)
        {
            _mqttClient = mqttClient;
            _logger = logger;
        }

        public async Task PublicarMensagemAsync(string topico, string mensagem)
        {
            if (!_mqttClient.IsConnected)
            {
                _logger.LogWarning("O cliente MQTT não está conectado. Tentando reconectar...");
                // Note: A conexão inicial é feita no MqttBackgroundService.
                // Aqui, apenas tentamos publicar, assumindo que o Background Service irá conectar.
                // Se a publicação falhar, o Controller irá tratar a exceção.
            }

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topico)
                .WithPayload(mensagem)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            _logger.LogInformation($"Publicando no tópico '{topico}': {mensagem}");
            
            // Publica, mesmo que a conexão não seja 100% garantida neste ponto.
            // A resiliência é gerenciada pelo background service.
            await _mqttClient.PublishAsync(message);
        }
    }
}