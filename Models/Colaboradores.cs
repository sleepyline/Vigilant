namespace VigiLant.Models
{
    public class Colaboradores
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }

        public string CPF { get; set; } // Ou outro identificador de funcionário

        public string Department { get; set; }

        public string JobTitle { get; set; }

        // Data de admissão
        public DateTime HireDate { get; set; }

        // Relacionamento 1:0..1 com o User (opcional)
        // Se for não-nulo, significa que este Colaborador tem acesso ao sistema
        public User SystemUser { get; set; }

        // Coleção de riscos que o colaborador pode ter identificado ou relatado
        public ICollection<Report> GeneratedReports { get; set; } = new List<Report>();
    }
}