namespace VigiLant.Models
{
    public class AnaliseIA
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "Solução" ou "Prevenção"
        public string Descricao { get; set; }
        public DateTime DataHoraAnalise { get; set; }
        public string RiscoRelacionado { get; set; } // Ex: "Superaquecimento da Caldeira"
        public string RiscoPrevisto { get; set; } // Usado apenas para Tipo="Prevenção"
    }
}
