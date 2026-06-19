using Microsoft.EntityFrameworkCore;
namespace BulkyWebBlazor_temp.Data
{
    public class ApplicationDbContext :DbContext 
    {
        public ApplicationDbContext(
         DbContextOptions<ApplicationDbContext> options)
            :base (options) { } 
        
        public DbSet<Models.Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Category>().HasData(
                new Models.Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Models.Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Models.Category { Id = 3, Name = "History", DisplayOrder = 3 }
            );
        }
    }
}
