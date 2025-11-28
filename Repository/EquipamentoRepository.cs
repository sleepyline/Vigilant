using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VigiLant.Data;
using VigiLant.Models;

namespace VigiLant.Repositories
{
    public class EquipamentoRepository : IEquipamentoRepository
    {
        private readonly BancoCtx _context;

        public EquipamentoRepository(BancoCtx context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Equipamento>> GetAllAsync()
        {
            return await _context.Equipamentos.ToListAsync();
        }

        public async Task<Equipamento> GetByIdAsync(int id)
        {
            return await _context.Equipamentos.FindAsync(id);
        }

        public async Task AddAsync(Equipamento equipamento)
        {
            _context.Equipamentos.Add(equipamento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Equipamento equipamento)
        {
            _context.Equipamentos.Update(equipamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var equipamento = await GetByIdAsync(id);
            if (equipamento != null)
            {
                _context.Equipamentos.Remove(equipamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
