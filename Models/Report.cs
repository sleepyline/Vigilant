namespace VigiLant.Models
{
    public class Report
    {
        public int ReportId { get; set; }

    // Tipo de relatório (ex: "Incidente", "Análise de Risco", "Auditoria")
    public string ReportType { get; set; }

    // Conteúdo ou detalhes do relatório
    public string Content { get; set; }

    // Data de geração do relatório
    public DateTime DateGenerated { get; set; }

    // Chave estrangeira para o Usuário que gerou o relatório
    public int UserId { get; set; }
    public User User { get; set; }

    // Chave estrangeira para o Risco que está sendo relatado/analisado
    public int RiskId { get; set; }
    public Risk Risk { get; set; }

    // Referência opcional a um equipamento (se o relatório for sobre ele)
    public int? EquipmentId { get; set; }
    public Equipment Equipment { get; set; }
    }
}