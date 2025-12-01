// Hubs/MedicaoHub.cs
using Microsoft.AspNetCore.SignalR;

namespace VigiLant.Hubs
{
    public class MedicaoHub : Hub
    {
        public Task EnviarMedicao(int equipamentoId, double corrente)
        {
            return Clients.All.SendAsync("ReceberMedicao", equipamentoId, corrente);
        }

        public Task NovoEquipamentoConectado(object equipamento)
        {
            return Clients.All.SendAsync("NovoEquipamentoConectado", equipamento);
        }
    }
}