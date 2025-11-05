// VigiLant.Repository/UsuarioRepository.cs

using VigiLant.Data;
using VigiLant.Models;
using Microsoft.EntityFrameworkCore;
using VigiLant.Contratos;

namespace VigiLant.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BancoCtx _context;

        public UsuarioRepository(BancoCtx context)
        {
            _context = context;
        }

        public async Task<Usuario> BuscarPorEmail(string email)
        {
            return await _context.Usuarios
                                 .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task Adicionar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }
    }
}