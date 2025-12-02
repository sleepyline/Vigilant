using VigiLant.Models; 

namespace VigiLant.Contratos
{
    public interface IRelatorioRepository
    {
        IEnumerable<Relatorio> GetAll();

        Relatorio GetById(int id);

        void Add(Relatorio relatorio);

        void Update(Relatorio relatorio);

        void Delete(int id);
    }
}