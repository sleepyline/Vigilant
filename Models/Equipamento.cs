// Models/Equipamento.cs
using System;
using System.ComponentModel.DataAnnotations;
using VigiLant.Models.Enum; 

namespace VigiLant.Models
{
    public class Equipamento
    {
        // Propriedades Básicas
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome do Equipamento")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A localização é obrigatória.")]
        [Display(Name = "Localização")]
        public string Localizacao { get; set; }

        // Propriedades de Conexão MQTT
        [Required(ErrorMessage = "O endereço do broker é obrigatório.")]
        [Display(Name = "Endereço do Broker (IP/Host)")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "A porta é obrigatória.")]
        [Range(1, 65535, ErrorMessage = "A porta deve ser entre 1 e 65535.")]
        [Display(Name = "Porta do Broker")]
        public int Porta { get; set; }

        [Required(ErrorMessage = "O tópico de medição é obrigatório.")]
        [Display(Name = "Tópico de Medição")]
        public string Topico { get; set; }
        
        public string TopicoResposta { get; set; }

        // Propriedades de Status e Manutenção

        public string Status { get; set; } 
        
        [DataType(DataType.Date)]
        [Display(Name = "Data da Última Manutenção")]
        public DateTime? DataUltimaManutencao { get; set; } 
        
        [Display(Name = "Tipo de Sensor")]
        public string TipoSensor { get; set; }
    }
}