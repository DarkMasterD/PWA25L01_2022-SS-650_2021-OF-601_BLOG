using Microsoft.EntityFrameworkCore;

namespace L01_NUMEROS_CARNETS.Models
{
    public class blogDBContext : DbContext
    {
        public blogDBContext(DbContextOptions<blogDBContext> options) : base(options)
        {

        }

        public DbSet<calificaciones> calificaciones { get; set; }
        public DbSet<comentarios> comentarios { get; set; }
        public DbSet<publicaciones> publicaciones { get; set; }
        public DbSet<roles> roles { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
    }
}
