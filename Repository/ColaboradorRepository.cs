using VigiLant.Contratos;
using VigiLant.Models;
using VigiLant.Data; 
using Microsoft.EntityFrameworkCore;

namespace VigiLant.Repository
{
    // A classe implementa a interface ajustada
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly BancoCtx _context;

        public ColaboradorRepository(BancoCtx context)
        {
            _context = context;
        }

        public IEnumerable<Colaborador> GetAll()
        {
            return _context.Colaboradores.ToList();
        }

        public Colaborador? GetById(int id)
        {
            return _context.Colaboradores.Find(id);
        }

        public void Add(Colaborador colaborador)
        {
            _context.Colaboradores.Add(colaborador);
            _context.SaveChanges();
        }

        public void Update(Colaborador colaborador)
        {
            _context.Entry(colaborador).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var colaborador = GetById(id);
            if (colaborador != null)
            {
                _context.Colaboradores.Remove(colaborador);
                _context.SaveChanges();
            }
        }
    }
}