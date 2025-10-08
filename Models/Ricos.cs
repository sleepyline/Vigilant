namespace VigiLant.Models
{
    public class Ricos
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string TipoRisco { get; set; } // Ex: Físico, Químico, Biológico, Ergonômico
        public string NivelSeveridade { get; set; } // Ex: Baixo, Médio, Alto
        public string AcoesMitigacao { get; set; }
        public bool RequerTreinamentoEspecifico { get; set; }
    }
}