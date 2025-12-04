using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using VigiLant.Contratos;
using VigiLant.Models.Payload; 
using VigiLant.Models.Enum; 

namespace VigiLant.Services
{
    public class MqttClientService : BackgroundService
    {
        private readonly ILogger<MqttClientService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IMqttClient _mqttClient;
        private MqttFactory _mqttFactory;

        // Propriedades para as configurações que serão carregadas do DB
        private string _mqttHost;
        private int _mqttPort;
        private string _mqttTopicWildcard;

        public MqttClientService(ILogger<MqttClientService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            
            _mqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
            _mqttClient.DisconnectedAsync += HandleDisconnectedAsync;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // O loop principal que garante que o serviço tente se conectar e se manter conectado
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_mqttClient.IsConnected)
                {
                    // 1. CARREGA AS CONFIGURAÇÕES DO BANCO DE DADOS (DENTRO DE UM SCOPE)
                    try
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var configRepository = scope.ServiceProvider.GetRequiredService<IAppConfigRepository>();
                            var config = configRepository.GetConfig();
                            
                            _mqttHost = config.MqttHost;
                            _mqttPort = config.MqttPort;
                            _mqttTopicWildcard = config.MqttTopicWildcard;
                            
                            _logger.LogInformation($"Configurações carregadas: Host={_mqttHost}, Topic={_mqttTopicWildcard}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao carregar configurações do AppConfigRepository. Tentando novamente em 10 segundos...");
                        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                        continue; 
                    }
                    
                    // 2. TENTA CONECTAR COM AS NOVAS CONFIGURAÇÕES
                    try
                    {
                        var options = new MqttClientOptionsBuilder()
                            .WithTcpServer(_mqttHost, _mqttPort) // Usa as configurações do DB
                            .WithClientId(Guid.NewGuid().ToString())
                            .WithCleanSession()
                            .Build();
                        
                        var result = await _mqttClient.ConnectAsync(options, stoppingToken);

                        if (result.ResultCode == MQTTnet.Client.MqttClientConnectResultCode.Success)
                        {
                            _logger.LogInformation($"Conectado ao Broker MQTT em {_mqttHost}:{_mqttPort}.");

                            // 3. SE INSCREVE NO TÓPICO CONFIGURÁVEL
                            await _mqttClient.SubscribeAsync(_mqttTopicWildcard, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce, stoppingToken);
                            _logger.LogInformation($"Subscrito ao tópico: {_mqttTopicWildcard}");
                        }
                        else
                        {
                            _logger.LogWarning($"Falha na conexão MQTT. Código: {result.ResultCode}. Tentando novamente em 5 segundos...");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao conectar ou subscrever ao Broker MQTT.");
                    }
                }
                
                // Espera 5 segundos antes de verificar a conexão novamente
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            _logger.LogWarning($"Desconectado do Broker MQTT. Tentando reconectar em 5 segundos...");
            // Não precisamos de um await aqui, o loop ExecuteAsync cuidará da reconexão.
            await Task.Delay(TimeSpan.FromSeconds(5)); 
        }

        private async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.ToArray());

            _logger.LogInformation($"Mensagem recebida no tópico '{topic}': {payload}");

            try
            {
                // Certifique-se de que a classe RealTimeDataPayload está acessível.
                var data = JsonConvert.DeserializeObject<RealTimeDataPayload>(payload);

                if (data == null || string.IsNullOrWhiteSpace(data.Identificador))
                {
                    _logger.LogWarning("Payload MQTT inválido ou Identificador ausente.");
                    return;
                }

                // Cria um escopo para resolver o IEquipamentoRepository (necessário para DbContext/Scoped services)
                using (var scope = _serviceProvider.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IEquipamentoRepository>();

                    // Busca o equipamento usando o IdentificadorBroker do payload
                    var equipamento = repo.GetAll().FirstOrDefault(eq => eq.IdentificadorBroker == data.Identificador);

                    if (equipamento != null)
                    {
                        repo.AtualizarDadosEmTempoReal(
                            equipamento.Id,
                            (StatusEquipament)data.Status,
                            data.Localizacao,
                            data.Nome,
                            (TipoSensores)data.TipoSensor
                        );
                        _logger.LogInformation($"Equipamento #{equipamento.Id} ({data.Identificador}) atualizado com dados reais.");
                    }
                    else
                    {
                        _logger.LogWarning($"Dados recebidos para identificador não cadastrado: {data.Identificador}");
                    }
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, $"Erro ao desserializar payload MQTT: {payload}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral ao processar mensagem MQTT.");
            }
            await Task.CompletedTask;
        }
    }
}