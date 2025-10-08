namespace VigiLant.Models
{
    public class Colaborador
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Cargo { get; set; }
        public string Contato { get; set; }
        public DateTime DataAdmissao { get; set; }
        public int SetorId { get; set; } // Chave estrangeira para o setor/departamento
    }
}