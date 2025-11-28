using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace VigiLant.Services
{
    public class MqttService : IMqttService
    {
        private readonly string _broker = "broker.emqx.io";
        private readonly int _port = 1883;

        public async Task<string> PublicarEReceberAsync(string topicoEnvio, string payload)
        {
            var factory = new MqttFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_broker, _port)
                .Build();

            string respostaRecebida = null;

            client.ApplicationMessageReceivedAsync += e =>
            {
                respostaRecebida = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                return Task.CompletedTask;
            };

            await client.ConnectAsync(options);

            // Inscreve no tópico de retorno (pode ajustar)
            await client.SubscribeAsync(topicoEnvio + "/resposta");

            // Publica para o sensor usando o tópico real
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topicoEnvio)
                .WithPayload(payload)
                .Build();

            await client.PublishAsync(message);

            // Aguarda resposta
            int tentativas = 0;
            while (respostaRecebida == null && tentativas < 30)
            {
                await Task.Delay(100);
                tentativas++;
            }

            await client.DisconnectAsync();

            return respostaRecebida;
        }
    }
}
