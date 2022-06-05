namespace SixMinApi.Data
{
    using Microsoft.EntityFrameworkCore;
    using SixMinApi.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
                
        }

        public DbSet<Command> Commands => Set<Command>();
    }
}
