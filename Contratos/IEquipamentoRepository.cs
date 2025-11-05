using VigiLant.Models;

namespace VigiLant.Contratos
{
    public interface IEquipamentoRepository
    {
        IEnumerable<Equipamento> GetAll();

        Equipamento GetById(int id);

        void Add(Equipamento equipamento);

        void Update(Equipamento equipamento);

        void Delete(int id);
    }
}