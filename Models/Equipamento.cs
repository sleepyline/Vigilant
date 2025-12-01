using System;
using System.ComponentModel.DataAnnotations;
using VigiLant.Models.Enum;

namespace VigiLant.Models
{
    public class Equipamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Tipo de Sensor")]
        public TipoSensores TipoSensor { get; set; }

       // Configurações MQTT
        [Required(ErrorMessage = "O Endereço do Broker é obrigatório.")]
        [Display(Name = "Endereço do Broker")]
        public string Endereco { get; set; }
        
        [Required(ErrorMessage = "A Porta é obrigatória.")]
        public int Porta { get; set; } = 1883; // Porta MQTT padrão

        [Required(ErrorMessage = "O Tópico de Medição é obrigatório.")]
        [Display(Name = "Tópico de Medição (SCT03 Data)")]
        public string Topico { get; set; }
        
        // NOVO: Onde o dispositivo enviará a confirmação "Conectado"
        public string TopicoResposta { get; set; }

        // Outras infos
        public string Localizacao { get; set; }
        public string Status { get; set; } = "Desconectado";

        [Display(Name = "Próxima Manutenção")]
        public DateTime DataUltimaManutencao { get; set; }
    }
}
