namespace VigiLant.Models
{
    public class Relatorio
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataGeracao { get; set; }
        public string Conteudo { get; set; } // Texto ou resumo do relatório
        public string TipoRelatorio { get; set; } // Ex: De Risco, De Incidente, De Manutenção
        public int GeradoPorColaboradorId { get; set; } // Chave estrangeira
    }
}