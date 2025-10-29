using VigiLant.Contratos;
using VigiLant.Models;
using VigiLant.Data; // Adicione este using para o seu DbContext
using Microsoft.EntityFrameworkCore; // Para usar métodos do EF Core

namespace VigiLant.Repository
{
    public class RiscoRepository : IRiscoRepository
    {
        private readonly BancoCtx _context;

        public RiscoRepository(BancoCtx context)
        {
            _context = context;
        }

        public IEnumerable<Risco> GetAll()
        {
            // Retorna todos os Riscos do DbSet e executa a consulta
            return _context.Riscos.ToList();
        }

        public Risco GetById(int id)
        {
            // Busca um Risco pela chave primária (Id)
            return _context.Riscos.Find(id);
        }

        public void Add(Risco risco)
        {
            _context.Riscos.Add(risco);
            // Salva as alterações no banco de dados
            _context.SaveChanges();
        }

        public void Update(Risco risco)
        {
            // Marca o objeto como Modificado
            _context.Entry(risco).State = EntityState.Modified;
            // Salva as alterações no banco de dados
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var risco = GetById(id);
            if (risco != null)
            {
                _context.Riscos.Remove(risco);
                // Salva as alterações no banco de dados
                _context.SaveChanges();
            }
        }
    }
}