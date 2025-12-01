using MQTTnet;
using MQTTnet.Client;
using System.Text;
using VigiLant.Contratos;

namespace VigiLant.Services
{
    public class MqttService : IMqttService
    {
        private readonly IMqttClient _client;
        private readonly MqttClientOptions _options;
        public event Action<string, string> OnMessageReceived;

        private readonly string BrokerAddress = "broker.emqx.io"; 

        public MqttService()
        {
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            _client.ConnectedAsync += async e =>
            {
                Console.WriteLine("Conectado ao MQTT!");
            };

            _client.DisconnectedAsync += async e =>
            {
                Console.WriteLine("Desconectado... tentando reconectar...");
                await Task.Delay(3000);
                await _client.ConnectAsync(_options);
            };

            _client.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                OnMessageReceived?.Invoke(topic, payload);
                return Task.CompletedTask;
            };

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(BrokerAddress, 1883)
                .WithClientId("Vigilant-" + Guid.NewGuid())
                .Build();
        }

        public async Task IniciarAsync()
        {
            if (!_client.IsConnected)
                await _client.ConnectAsync(_options);
        }

        public async Task PublicarAsync(string topico, string mensagem)
        {
            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(topico)
                .WithPayload(mensagem)
                .Build();

            await _client.PublishAsync(msg);
        }

        public async Task AssinarTopicoAsync(string topico)
        {
            await _client.SubscribeAsync(topico);
        }

        public async Task DesassinarTopicoAsync(string topico)
        {
            await _client.UnsubscribeAsync(topico);
        }

        public Task FinalizarAsync() => _client.DisconnectAsync();
    }
}
