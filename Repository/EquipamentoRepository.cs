using VigiLant.Contratos;
using VigiLant.Models;
using VigiLant.Data; // Adicione este using para o seu DbContext
using Microsoft.EntityFrameworkCore; // Para usar métodos do EF Core

namespace VigiLant.Repository
{
    public class EquipamentoRepository : IEquipamentoRepository
    {
        private readonly BancoCtx _context;

        public EquipamentoRepository(BancoCtx context)
        {
            _context = context;
        }

        public IEnumerable<Equipamento> GetAll()
        {
            return _context.Equipamentos.ToList();
        }

        public Equipamento GetById(int id)
        {
            return _context.Equipamentos.Find(id);
        }

        public void Add(Equipamento equipamento)
        {
            _context.Equipamentos.Add(equipamento);
            _context.SaveChanges();
        }

        public void Update(Equipamento equipamento)
        {
            _context.Entry(equipamento).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var equipamento = GetById(id);
            if (equipamento != null)
            {
                _context.Equipamentos.Remove(equipamento);
                _context.SaveChanges();
            }
        }
    }
}