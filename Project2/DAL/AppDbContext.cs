using Microsoft.EntityFrameworkCore;
using Project2.Models;

namespace Project2.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<Log> Logs { get; set; }
	}
}
