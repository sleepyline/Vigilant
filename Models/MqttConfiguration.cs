using System.ComponentModel.DataAnnotations;

namespace VigiLant.Models
{
    public class MqttConfiguration
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O endereço do Broker é obrigatório.")]
        [Display(Name = "Endereço (Host/IP)")]
        public string BrokerHost { get; set; }

        [Required(ErrorMessage = "A porta do Broker é obrigatória.")]
        [Display(Name = "Porta")]
        public int BrokerPort { get; set; }

        [Required(ErrorMessage = "O tópico base é obrigatório (Ex: vigilant/sensores).")]
        [Display(Name = "Tópico Base")]
        public string BaseTopic { get; set; } = "vigilant/sensores";
    }
}