namespace VigiLant.Models
{
    public class Equipamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NumeroSerie { get; set; }
        public DateTime DataAquisicao { get; set; }
        public string Localizacao { get; set; }
        public bool EmManutencao { get; set; }
        

        public int ColaboradorResponsavelId { get; set; } // Chave estrangeira
    }
}