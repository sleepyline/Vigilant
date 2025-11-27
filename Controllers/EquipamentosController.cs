using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VigiLant.Contratos;
using VigiLant.Models;

namespace VigiLant.Controllers
{
    [Authorize]
    public class EquipamentosController : Controller
    {
        private readonly IEquipamentoRepository _equipamentoRepository;
        private readonly IMqttConfigurationRepository _configRepository;
        private readonly IMqttService _mqttService; // Novo serviço injetado

        // Construtor atualizado para injetar os novos serviços
        public EquipamentosController(IEquipamentoRepository equipamentoRepository, 
                                      IMqttConfigurationRepository configRepository, 
                                      IMqttService mqttService) 
        {
            _equipamentoRepository = equipamentoRepository;
            _configRepository = configRepository;
            _mqttService = mqttService;
        }

        public IActionResult Index()
        {
            var equipamentos = _equipamentoRepository.GetAll();
            return View(equipamentos);
        }
        
        // GET: /Equipamentos/Conectar (Adicionar esta View, ela usa o Equipamento Model)
        public IActionResult Conectar()
        {
            var config = _configRepository.GetConfiguration();
            // Verifica se as configurações globais foram preenchidas
            if (string.IsNullOrEmpty(config.BrokerHost))
            {
                TempData["ErrorMessage"] = "Por favor, configure o Broker MQTT antes de conectar um sensor.";
                return RedirectToAction("Index", "Configuracoes");
            }
            
            // Retorna a view 'Conectar' com um novo objeto Equipamento
            return View(new Equipamento 
            {
                BrokerHost = config.BrokerHost,
                BrokerPort = config.BrokerPort,
                // O Tópico será gerado no POST, mas Host e Porta são pré-preenchidos.
                MqttTopic = config.BaseTopic
            });
        }

        // POST: /Equipamentos/Conectar (Lógica de Teste e Salvar)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Conectar(Equipamento novoSensor)
        {
            // O Host e Porta vêm da View (que foram preenchidos globalmente, mas permitem alteração)
            
            // 1. Geração Dinâmica do Tópico
            // Formato: BaseTopic/Tipo/NomeSensor
            var baseConfig = _configRepository.GetConfiguration();
            
            // Tratamento básico de nomes para tópicos
            var sensorType = novoSensor.TipoSensor?.Replace(" ", "").ToLower();
            var sensorName = novoSensor.Nome?.Replace(" ", "").ToLower();
            
            // Gera o tópico final: vigilant/sensores/vibracao/sensor1
            novoSensor.MqttTopic = $"{baseConfig.BaseTopic.TrimEnd('/')}/{sensorType}/{sensorName}";
            
            // Define a localização padrão
            if (string.IsNullOrEmpty(novoSensor.Localizacao))
            {
                novoSensor.Localizacao = $"MQTT: {novoSensor.MqttTopic}";
            }

            // Garante que a porta e host estão preenchidos para a validação do Model
            novoSensor.BrokerHost = baseConfig.BrokerHost;
            novoSensor.BrokerPort = baseConfig.BrokerPort;


            if (ModelState.IsValid)
            {
                // 2. Executar o Teste de Conexão MQTT
                // A mensagem esperada do sensor para confirmar a conexão é "CONECTADO"
                const string expectedMessage = "CONECTADO"; 
                
                // NOTA: Esta chamada simulará a escuta do tópico no broker real.
                bool isConnected = await _mqttService.TestConnectionAndSubscribeAsync(
                    novoSensor.BrokerHost, 
                    novoSensor.BrokerPort, 
                    novoSensor.MqttTopic, 
                    expectedMessage,
                    novoSensor.Nome
                );

                if (isConnected)
                {
                    try
                    {
                        // 3. Salvar o novo Equipamento/Sensor
                        _equipamentoRepository.Add(novoSensor);

                        // Notificação de sucesso
                        TempData["SuccessMessage"] = $"Sensor **{novoSensor.Nome}** conectado e verificado no Broker! Monitoramento iniciado no Tópico: `{novoSensor.MqttTopic}`";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Conexão MQTT OK, mas erro ao salvar no banco: {ex.Message}");
                    }
                }
                else
                {
                    // Falha no teste de conexão
                    ModelState.AddModelError("", $"Falha ao confirmar a conexão no Tópico: `{novoSensor.MqttTopic}`. O sensor deve publicar a mensagem '{expectedMessage}' para ser salvo.");
                }
            }
            
            // Se houver erro, retorna para a View com os dados preenchidos
            return View(novoSensor);
        }
    }
}