// VigiLant.Contratos/IEquipamentoRepository.cs
using VigiLant.Models;
using System.Collections.Generic;

namespace VigiLant.Contratos
{
    public interface IEquipamentoRepository
    {
        void Add(Equipamento equipamento);
        void Update(Equipamento equipamento);
        void Delete(int id);
        Equipamento GetById(int id);
        IEnumerable<Equipamento> GetAll();
    }
}