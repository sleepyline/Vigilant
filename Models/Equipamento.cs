namespace VigiLant.Models
{
    public class Equipamento
    {

        public int Id { get; set; }

        public string Nome { get; set; }

        public string TipoSensor { get; set; }

        public string Status { get; set; }

        public string Localizacao { get; set; } 

        public DateTime DataUltimaManutencao { get; set; }


        // Configurações do Broker MQTT (Para o backend saber onde escutar)
        public string BrokerHost { get; set; }
        public int BrokerPort { get; set; }
        public string MqttTopic { get; set; }
        public string MqttUser { get; set; }   
        public string MqttPassword { get; set; }
    }
}