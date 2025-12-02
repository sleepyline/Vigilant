using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using VigiLant.Contratos;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace VigiLant.Services
{
    // IHostedService: Permite que este serviço rode em background continuamente
    public class MqttBackgroundService : BackgroundService
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttFactory _mqttFactory;
        private readonly ILogger<MqttBackgroundService> _logger;
        // Permite acessar serviços scoped (como IEquipamentoRepository) dentro de um Singleton
        private readonly IServiceScopeFactory _serviceScopeFactory; 

        // Tópico para subscrição de respostas de conexão de TODOS os equipamentos
        private const string ConnectionResponseTopic = "+/resposta"; 

        public MqttBackgroundService(
            IMqttClient mqttClient, 
            MqttFactory mqttFactory, 
            ILogger<MqttBackgroundService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _mqttClient = mqttClient;
            _mqttFactory = mqttFactory;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço de Background MQTT iniciado.");

            // Configuração e conexão do cliente MQTT
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.emqx.io", 1883)
                .WithClientId($"VigiLant_Server_{Guid.NewGuid()}")
                .WithCleanSession()
                .Build();
            
            _mqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
            
            // Lógica de reconexão contínua
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!_mqttClient.IsConnected)
                    {
                        var result = await _mqttClient.ConnectAsync(options, stoppingToken);
                        if (result.ResultCode == MqttClientConnectResultCode.Success)
                        {
                            _logger.LogInformation("Conectado ao broker MQTT com sucesso.");
                            // Subscreve ao tópico genérico de respostas
                            await _mqttClient.SubscribeAsync(ConnectionResponseTopic);
                            _logger.LogInformation($"Subscrito ao tópico: {ConnectionResponseTopic}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao conectar ou subscrever ao broker MQTT. Tentando novamente em 5 segundos.");
                }

                // Espera antes de tentar reconectar/re-verificar
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); 
            }
        }
        
        // Manipulador de Mensagens Recebidas
        private async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            // O payload esperado deve ser: {"Status": "Conectado", "TipoSensor": "Temperatura"}
            // O tópico de origem é algo como: "sensor/temp/labx/resposta"
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            var topic = e.ApplicationMessage.Topic;
            
            _logger.LogInformation($"Mensagem recebida no tópico {topic}: {payload}");

            try
            {
                // Extrai o ID/Identificador do Equipamento do Tópico (Ex: "sensor/temp/labx" a partir de "sensor/temp/labx/resposta")
                var baseTopic = topic.Replace("/resposta", string.Empty);

                // IMPORTANTE: Aqui você usará o Repositório para atualizar o Equipamento.
                // Como IEquipamentoRepository é Scoped/Transient, precisa ser resolvido via IServiceScopeFactory.
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IEquipamentoRepository>();
                    
                    // Lógica para encontrar o Equipamento
                    // 1. Busca pelo Equipamento que tenha o TopicoResposta correspondente.
                    var equipamento = repo.GetAll().FirstOrDefault(eq => eq.TopicoResposta == topic); 
                    
                    if (equipamento != null)
                    {
                        // DESERIALIZAÇÃO DA RESPOSTA DO SENSOR
                        // O sensor deve mandar um JSON como: {"Status": "Conectado", "TipoSensor": "Temperatura"}
                        var sensorResponse = JsonSerializer.Deserialize<SensorResponse>(payload);

                        if (sensorResponse.Status == "Conectado")
                        {
                            // ATUALIZA O EQUIPAMENTO NO BANCO DE DADOS
                            equipamento.Status = "Ativo";
                            equipamento.TipoSensor = sensorResponse.TipoSensor;
                            repo.Update(equipamento);
                            _logger.LogInformation($"Equipamento ID {equipamento.Id} ({equipamento.Nome}) CONECTADO e ATUALIZADO com TipoSensor: {equipamento.TipoSensor}");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Equipamento não encontrado para o tópico de resposta: {topic}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem MQTT.");
            }
            
            await Task.CompletedTask;
        }
        
        // Model auxiliar para desserializar a resposta do sensor
        public class SensorResponse
        {
            public string Status { get; set; }
            public string TipoSensor { get; set; }
        }
    }
}