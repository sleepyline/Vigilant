using VigiLant.Models;

namespace VigiLant.Contratos
{
    public interface IMqttConfigurationRepository
    {
        MqttConfiguration GetConfiguration();
        void SaveConfiguration(MqttConfiguration config);
    }
}