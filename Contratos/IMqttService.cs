// Contratos/IMqttService.cs
using System.Threading.Tasks;

namespace VigiLant.Contratos
{
    public interface IMqttService
    {
        event Action<string, string> OnMessageReceived;
        
        Task PublicarAsync(string topico, string mensagem);
        
        Task IniciarAsync();
        
        Task FinalizarAsync();
        
        Task AssinarTopicoAsync(string topico);
        
        Task DesassinarTopicoAsync(string topico);
    }
}