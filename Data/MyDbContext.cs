using Microsoft.EntityFrameworkCore;

namespace ProjetoMIragnum
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base (options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
