using System.ComponentModel.DataAnnotations;
using VigiLant.Models.Enum;

namespace VigiLant.Models
{
    public class Risco
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TipoRisco Tipo { get; set; } // Ex: Físico, Químico, Biológico, Ergonômico
        public NivelSeveridade Nivel { get; set; } // Ex: Baixo, Médio, Alto
        public string AcoesMitigacao { get; set; }
        public bool RequerTreinamentoEspecifico { get; set; }
    }
}