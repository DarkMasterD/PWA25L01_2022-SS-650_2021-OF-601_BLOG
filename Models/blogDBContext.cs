using Microsoft.EntityFrameworkCore;

namespace L01_NUMEROS_CARNETS.Models
{
    public class blogDBContext : DbContext
    {
        public blogDBContext(DbContextOptions<blogDBContext> options) : base(options)
        {

        }
    }
}
