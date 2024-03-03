using Microsoft.EntityFrameworkCore;

namespace MyWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

         public DbSet<User> User { get; set; }
         public DbSet<Question> Question { get; set; }

         public DbSet<QuizSession> QuizSession { get; set; }

        // DbSet properties for your entities
        // public DbSet<User> Users { get; set; }
        // public DbSet<Product> Products { get; set; }
    }
}
