namespace VigiLant.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; } // Ex: "Admin", "Gerente de Risco", "Colaborador"

        // Descrição das permissões deste papel
        public string PermissionsDescription { get; set; }

        // Coleção de usuários que possuem este papel
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}