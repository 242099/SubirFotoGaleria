using Microsoft.EntityFrameworkCore;
using BackendFotosApi.Models;

namespace BackendFotosApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Asegúrate de que use 'Fotografia' (singular) si tu modelo se llama así
        public DbSet<Fotografia> Fotografias { get; set; }
    }
}