using Microsoft.EntityFrameworkCore;
using mvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Employee> Employees { get; set; }
                protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>()
                .HasDiscriminator<string>("PersonType") 
                .HasValue<Person>("Person")
                .HasValue<Student>("Student");
        }
    }
}
