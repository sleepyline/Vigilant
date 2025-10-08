using Microsoft.EntityFrameworkCore;
using VigiLant.Models;

namespace VigiLant.Data
{
    public class BancoCtx : DbContext
    {
        public BancoCtx(DbContextOptions<BancoCtx> options) : base(options)
        {

        }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<Risco> Riscos { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        
    }
}