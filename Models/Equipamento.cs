namespace VigiLant.Models
{
    public class Equipamento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataManutencao { get; set; }
    }
}