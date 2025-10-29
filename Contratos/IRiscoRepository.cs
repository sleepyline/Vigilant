using VigiLant.Models;

namespace VigiLant.Contratos
{
    public interface IRiscoRepository
    {
        IEnumerable<Risco> GetAll();

        // Recupera um risco pelo ID
        Risco GetById(int id);

        // Adiciona um novo risco
        void Add(Risco risco);

        // Atualiza um risco existente
        void Update(Risco risco);

        // Remove um risco pelo ID
        void Delete(int id);
    }
}