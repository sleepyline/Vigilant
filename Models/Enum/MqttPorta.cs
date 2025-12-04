// Models/Enum/MqttPorta.cs

using System.ComponentModel.DataAnnotations;

namespace VigiLant.Models.Enum
{
    public enum MqttPorta
    {
        [Display(Name = "1883 - Padrão TCP (Insegura)")]
        Porta1883 = 1883,
        
        [Display(Name = "8883 - Padrão SSL/TLS (Segura)")]
        Porta8883 = 8883,

        [Display(Name = "8080 - WebSocket (Insegura)")]
        Porta8080 = 8080,
        
        [Display(Name = "8083 - WebSocket SSL/TLS (Segura)")]
        Porta8083 = 8083
    }
}