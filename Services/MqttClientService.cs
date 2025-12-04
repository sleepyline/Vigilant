using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VigiLant.Contratos;
using VigiLant.Models.Enum;
using Microsoft.Extensions.DependencyInjection;
using System.Linq; // Necessário para o Linq no HandleApplicationMessageReceivedAsync

namespace VigiLant.Services
{
    public class MqttClientService : BackgroundService
    {
        private readonly IMqttClient _mqttClient;
        private readonly ILogger<MqttClientService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private const string BrokerHost = "broker.emqx.io";
        private const string TopicWildcard = "vigilant/data/#"; // Assina todos os sensores

        public MqttClientService(ILogger<MqttClientService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            // Configuração dos handlers
            _mqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
            _mqttClient.DisconnectedAsync += HandleDisconnectedAsync;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(OnStopping);
            await ConnectAndSubscribeAsync(stoppingToken);
        }

        private async Task ConnectAndSubscribeAsync(CancellationToken stoppingToken)
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(BrokerHost, 1883)
                .WithClientId($"VigiLantApp_{Guid.NewGuid()}")
                .WithCleanSession()
                .Build();

            try
            {
                var result = await _mqttClient.ConnectAsync(options, stoppingToken);
                _logger.LogInformation($"Conectado ao Broker EMQX: {result.ResultCode}");

                // Subscrição ao tópico de dados
                await _mqttClient.SubscribeAsync(TopicWildcard, MqttQualityOfServiceLevel.AtLeastOnce, stoppingToken);
                _logger.LogInformation($"Subscrito ao tópico: {TopicWildcard}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao conectar ou subscrever ao Broker: {ex.Message}");
                // Tenta reconectar após 5 segundos, a menos que o token de parada seja cancelado.
                if (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    await ConnectAndSubscribeAsync(stoppingToken);
                }
            }
        }

        private async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            _logger.LogWarning($"Desconectado do Broker. Tentando reconectar...");
            await Task.Delay(TimeSpan.FromSeconds(5));
            await ConnectAndSubscribeAsync(CancellationToken.None);
        }

        private async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            _logger.LogInformation($"Mensagem recebida no tópico {e.ApplicationMessage.Topic}: {payload}");

            try
            {
                // DESERIALIZAÇÃO DO JSON
                var data = JsonSerializer.Deserialize<RealTimeDataPayload>(payload);

                if (data == null) return;

                // CRIA UM NOVO ESCOPO para usar o BancoCtx (Scoped) dentro do HostedService (Singleton)
                using (var scope = _serviceProvider.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IEquipamentoRepository>();

                    // Busca o equipamento pelo IdentificadorBroker
                    var equipamento = repo.GetAll().FirstOrDefault(eq => eq.IdentificadorBroker == data.Identificador);

                    if (equipamento != null)
                    {
                        // Mapeamento dos enums (se necessário, adicione validação de valores)
                        var status = Enum.IsDefined(typeof(StatusEquipament), data.Status) ? (StatusEquipament)data.Status : StatusEquipament.Erro;
                        var tipoSensor = Enum.IsDefined(typeof(TipoSensores), data.TipoSensor) ? (TipoSensores)data.TipoSensor : TipoSensores.Temperatura; // Ajuste o default caso TipoSensores não tenha Umidade

                        // Atualiza o DB com os dados REAIS
                        repo.AtualizarDadosEmTempoReal(
                            equipamento.Id,
                            status,
                            data.Localizacao ?? equipamento.Localizacao, // Usa o dado novo, ou mantém o antigo se for null
                            data.Nome ?? equipamento.Nome,
                            tipoSensor
                        );
                        _logger.LogInformation($"Equipamento #{equipamento.Id} atualizado com dados reais.");
                    }
                    else
                    {
                        _logger.LogWarning($"Dados recebidos para identificador não cadastrado: {data.Identificador}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar mensagem MQTT: {ex.Message}");
            }
        }

        private void OnStopping()
        {
            _logger.LogInformation("Serviço MQTT encerrando.");
            // CORREÇÃO DE SINTAXE: Disconexão limpa e assíncrona
            _mqttClient.DisconnectAsync(new MqttClientDisconnectOptions()).Wait();
        }
    }

    // Modelo de dados esperado do seu dispositivo embarcado
    public class RealTimeDataPayload
    {
        public string Identificador { get; set; } // O TOKEN/ID usado no Conectar
        public string? Nome { get; set; }
        public string? Localizacao { get; set; }
        public int TipoSensor { get; set; } // Valor inteiro do Enum TipoSensor
        public int Status { get; set; }     // Valor inteiro do Enum EquipamentoStatus

    }
}