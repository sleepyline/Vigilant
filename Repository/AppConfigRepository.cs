// Repository/AppConfigRepository.cs

using VigiLant.Contratos;
using VigiLant.Models;
using VigiLant.Data;
using System.Linq;

namespace VigiLant.Repository
{
    public class AppConfigRepository : IAppConfigRepository
    {
        private readonly BancoCtx _context;

        public AppConfigRepository(BancoCtx context)
        {
            _context = context;
        }

        public AppConfig GetConfig()
        {
            // Tenta obter a configuração existente (sempre Id=1)
            var config = _context.AppConfigs.FirstOrDefault(c => c.Id == 1);

            if (config == null)
            {
                // Se não existir, cria o registro inicial com os valores default
                config = new AppConfig();
                _context.AppConfigs.Add(config);
                _context.SaveChanges();
            }

            return config;
        }

        public void UpdateConfig(AppConfig config)
        {
            var existing = _context.AppConfigs.FirstOrDefault(c => c.Id == 1);
            if (existing != null)
            {
                existing.MqttHost = config.MqttHost;
                existing.MqttTopicWildcard = config.MqttTopicWildcard;
                existing.MqttPort = config.MqttPort;
                _context.AppConfigs.Update(existing);
                _context.SaveChanges();
            }
        }
    }
}