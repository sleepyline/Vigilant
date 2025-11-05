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
            return _context.Riscos.ToList();
        }

        public Risco GetById(int id)
        {
            return _context.Riscos.Find(id);
        }

        public void Add(Risco risco)
        {
            _context.Riscos.Add(risco);
            _context.SaveChanges();
        }

        public void Update(Risco risco)
        {
            _context.Entry(risco).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var risco = GetById(id);
            if (risco != null)
            {
                _context.Riscos.Remove(risco);
                _context.SaveChanges();
            }
        }
    }
}