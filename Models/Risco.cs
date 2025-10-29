using System.ComponentModel.DataAnnotations;
using VigiLant.Models.Enum;

namespace VigiLant.Models
{
    public class Risco
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public NivelSeveridade NivelGravidade { get; set; } 
        public string Status { get; set; } = string.Empty;
        public DateTime DataIdentificacao { get; set; }
    }
}