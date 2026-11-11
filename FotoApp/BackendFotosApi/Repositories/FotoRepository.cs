using BackendFotosApi.Data;
using BackendFotosApi.Models;

namespace BackendFotosApi.Repositories
{
    public class FotoRepository
    {
        private readonly AppDbContext _context;

        public FotoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(Fotografia fotografia) // <-- Debe ser el tipo singular exacto
        {
            _context.Fotografias.Add(fotografia);
            await _context.SaveChangesAsync();
        }
    }
}