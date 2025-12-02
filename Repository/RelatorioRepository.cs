using VigiLant.Contratos;
using VigiLant.Models;
using VigiLant.Data; // Assumindo o mesmo DbContext
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VigiLant.Repository
{
    public class RelatorioRepository : IRelatorioRepository
    {

        private readonly BancoCtx _context;

        public RelatorioRepository(BancoCtx context)
        {
            _context = context;
        }

        private int GetNextAvailableId()
        {
            var existingIds = _context.Relatorios 
                                    .Select(r => r.Id)
                                    .OrderBy(id => id)
                                    .ToList();

            if (!existingIds.Any())
            {
                return 1;
            }

            int nextId = 1;
            
            foreach (var id in existingIds)
            {
                if (id > nextId)
                {
                    return nextId; 
                }
                nextId = id + 1;
            }

            return nextId;
        }


        public IEnumerable<Relatorio> GetAll()
        {
            return _context.Relatorios.ToList();
        }

        public Relatorio GetById(int id)
        {
            return _context.Relatorios.Find(id);
        }

        public void Add(Relatorio relatorio)
        {
            relatorio.Id = GetNextAvailableId(); 
            _context.Relatorios.Add(relatorio);
            _context.SaveChanges(); 
        }

        public void Update(Relatorio relatorio)
        {
            _context.Entry(relatorio).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var relatorio = GetById(id);
            if (relatorio != null)
            {
                _context.Relatorios.Remove(relatorio);
                _context.SaveChanges(); 
            }
        }
    }
}