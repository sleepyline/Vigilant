using VigiLant.Contratos;
using VigiLant.Data;
using VigiLant.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VigiLant.Repository
{
    // Implementação singleton (um único registro) para configurações globais.
    public class MqttConfigurationRepository : IMqttConfigurationRepository
    {
        private readonly BancoCtx _context;
        private const int CONFIG_ID = 1; // ID fixo para o registro único

        public MqttConfigurationRepository(BancoCtx context)
        {
            _context = context;
        }

        public MqttConfiguration GetConfiguration()
        {
            // Tenta obter a configuração existente (ID=1) ou retorna um novo objeto
            return _context.MqttConfigurations.Find(CONFIG_ID) 
                   ?? new MqttConfiguration { Id = CONFIG_ID };
        }

        public void SaveConfiguration(MqttConfiguration config)
        {
            var existingConfig = _context.MqttConfigurations.Find(CONFIG_ID);

            if (existingConfig == null)
            {
                // Se não existe, adiciona
                config.Id = CONFIG_ID;
                _context.MqttConfigurations.Add(config);
            }
            else
            {
                // Se existe, atualiza
                existingConfig.BrokerHost = config.BrokerHost;
                existingConfig.BrokerPort = config.BrokerPort;
                existingConfig.BaseTopic = config.BaseTopic;
                _context.Entry(existingConfig).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }
    }
}