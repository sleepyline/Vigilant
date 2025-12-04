// Models/AppConfig.cs

using System.ComponentModel.DataAnnotations;

namespace VigiLant.Models
{
    public class AppConfig
    {
        public int Id { get; set; } = 1; 

        public string MqttHost { get; set; } = "broker.emqx.io"; 
        public string MqttTopicWildcard { get; set; } = "vigilant/data/#"; 
        public int MqttPort { get; set; } = 1883; 
    }
}