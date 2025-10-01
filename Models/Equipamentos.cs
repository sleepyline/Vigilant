namespace VigiLant.Models
{
    public class Equipamentos
    {
        public int EquipmentId { get; set; }

        // Nome ou modelo do equipamento
        public string Name { get; set; }

        // Número de série ou TAG de identificação
        public string SerialNumber { get; set; }

        // Localização atual do equipamento
        public string Location { get; set; }

        // Data da última manutenção
        public DateTime? LastMaintenanceDate { get; set; }

        // Status (ex: Em operação, Em manutenção, Desativado)
        public string Status { get; set; }

        // Coleção de riscos associados a este equipamento
        public ICollection<Risk> AssociatedRisks { get; set; } = new List<Risk>();
        // Observação: Para ligar Equipamento a Risco, você pode precisar de uma 
        // classe de junção (RiskEquipment) ou simplesmente fazer a associação na classe Report.
        // Para simplificar, vou criar a relação na classe Report.
    }
}