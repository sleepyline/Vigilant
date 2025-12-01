using Microsoft.AspNetCore.SignalR;
using VigiLant.Contratos;
using VigiLant.Hubs;
using VigiLant.Models;
using VigiLant.Repositories;

namespace VigiLant.Services
{
    public class MqttHostedService : IHostedService, IDisposable
    {
        private readonly IMqttService _mqttService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<MedicaoHub> _hubContext;
        private Timer _timer;

        public MqttHostedService(IMqttService mqttService, IServiceProvider serviceProvider, IHubContext<MedicaoHub> hubContext)
        {
            _mqttService = mqttService;
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _mqttService.IniciarAsync();
            _mqttService.OnMessageReceived += OnMensagemMqttRecebida;

            // Assina todos os tópicos de equipamentos existentes no banco de dados
            using (var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IEquipamentoRepository>();
                var equipamentos = await repo.GetAllAsync();
                
                foreach (var eq in equipamentos.Where(e => !string.IsNullOrEmpty(e.Topico)))
                {
                    await _mqttService.AssinarTopicoAsync(eq.Topico);
                    if (!string.IsNullOrEmpty(eq.TopicoResposta))
                    {
                        await _mqttService.AssinarTopicoAsync(eq.TopicoResposta);
                    }
                }
            }

            // Exemplo de Timer para reconexão/manutenção (Opcional, mas útil)
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        private void DoWork(object state)
        {
            // Lógica de monitoramento (Ex: verificar status da conexão)
            // Console.WriteLine($"Monitoramento MQTT: {DateTime.Now}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _mqttService.FinalizarAsync();
            _mqttService.OnMessageReceived -= OnMensagemMqttRecebida;
            return Task.CompletedTask;
        }

        private async void OnMensagemMqttRecebida(string topico, string mensagem)
        {
            // Criar um escopo para acessar o repositório no Hosted Service
            using (var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IEquipamentoRepository>();
                
                // Busca o equipamento pelo Tópico (Medição) ou TópicoResposta (Status)
                var equipamento = (await repo.GetAllAsync())
                                    .FirstOrDefault(e => e.Topico == topico || e.TopicoResposta == topico);
                
                if (equipamento == null) return;

                // 1. Lógica de Confirmação de Conexão (Tópico de Resposta)
                if (topico == equipamento.TopicoResposta && mensagem.Equals("Conectado", StringComparison.OrdinalIgnoreCase))
                {
                    equipamento.Status = "Conectado";
                    await repo.UpdateAsync(equipamento);
                    
                    // Notifica todos os clientes web sobre o novo equipamento conectado
                    await _hubContext.Clients.All.SendAsync("NovoEquipamentoConectado", equipamento);
                    return;
                }

                // 2. Lógica de Medição em Tempo Real (Tópico de Medição)
                if (topico == equipamento.Topico && double.TryParse(mensagem, out double corrente))
                {
                    // Envia a medição para o SignalR Hub
                    await _hubContext.Clients.All.SendAsync("ReceberMedicao", equipamento.Id, corrente);

                    // Atualiza o status se for a primeira medição (Opcional)
                    if (equipamento.Status != "Em Operação")
                    {
                        equipamento.Status = "Em Operação";
                        await repo.UpdateAsync(equipamento);
                    }
                }
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}