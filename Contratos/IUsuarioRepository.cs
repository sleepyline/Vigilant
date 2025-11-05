// VigiLant.Contratos/IUsuarioRepository.cs

using VigiLant.Models;
using System.Threading.Tasks;

public interface IUsuarioRepository
{
    Task<Usuario> BuscarPorEmail(string email);
    Task Adicionar(Usuario usuario);
}