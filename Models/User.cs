namespace VigiLant.Models
{
    public class User
    {
         public int UserId { get; set; }

    // Nome de usuário para login
    public string Username { get; set; }

    // Hash da senha (NUNCA armazene a senha em texto claro!)
    public string PasswordHash { get; set; }

    public string Email { get; set; }

    // Chave estrangeira para o Papel (Role)
    public int RoleId { get; set; }
    public Role Role { get; set; }

    // Referência opcional ao registro de Colaborador (se for a mesma pessoa)
    public int? EmployeeId { get; set; }
    public Colaboradores Employee { get; set; }

    // Coleções de itens sob responsabilidade deste usuário
    public ICollection<Riscos> ManagedRisks { get; set; } = new List<Riscos>();
    public ICollection<Report> GeneratedReports { get; set; } = new List<Report>();
    }
}