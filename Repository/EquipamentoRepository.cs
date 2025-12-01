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
        private int GetNextAvailableId()
        {
            // 1. Busca todos os IDs existentes e os ordena
            var existingIds = _context.Equipamentos
                                    .Select(r => r.Id)
                                    .OrderBy(id => id)
                                    .ToList();

            if (!existingIds.Any())
            {
                return 1; // Tabela vazia, começa em 1
            }

            int nextId = 1;

            // 2. Itera para encontrar o primeiro ID faltante (o buraco)
            foreach (var id in existingIds)
            {
                if (id > nextId)
                {
                    // Ex: IDs são {1, 3, 4}. nextId é 2, id é 3. Retorna 2.
                    return nextId;
                }
                nextId = id + 1; // Move para o próximo ID sequencial
            }

            // 3. Se não houver buracos (Ex: 1, 2, 3), retorna o próximo sequencial
            return nextId;
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
            equipamento.Id = GetNextAvailableId();
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
