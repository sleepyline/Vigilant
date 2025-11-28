using System;

namespace VigiLant.Models
{
    public class Equipamento
    {
        public int Id { get; set; }

        public string Nome { get; set; }
        public string TipoSensor { get; set; }

        // MQTT
        public string Topico { get; set; }
        public string Endereco { get; set; }
        public int Porta { get; set; }

        // Outras infos
        public string Localizacao { get; set; }
        public string Status { get; set; }

        public DateTime DataUltimaManutencao { get; set; }
    }
}
