using System.Collections.Generic;
using System.Threading.Tasks;
using VigiLant.Models;

namespace VigiLant.Repositories
{
    public interface IEquipamentoRepository
    {
        Task<IEnumerable<Equipamento>> GetAllAsync();
        Task<Equipamento> GetByIdAsync(int id);
        Task AddAsync(Equipamento equipamento);
        Task UpdateAsync(Equipamento equipamento);
        Task DeleteAsync(int id);
    }
}
