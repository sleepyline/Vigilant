using VigiLant.Models;

namespace VigiLant.Contratos
{
    // Contrato para o serviço de comunicação MQTT, focado no teste de conexão.
    public interface IMqttService
    {
        // Tenta se conectar e se inscrever no tópico de teste, esperando uma mensagem de "conectado"
        Task<bool> TestConnectionAndSubscribeAsync(string host, int port, string topic, string expectedMessage, string sensorName);
    }
}