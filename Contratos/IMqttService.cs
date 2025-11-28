using System.Threading.Tasks;

namespace VigiLant.Services
{
    public interface IMqttService
    {
        Task<string> PublicarEReceberAsync(string topicoEnvio, string payload);
    }
}
