using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    //public class AppDbContext : IdentityDbContext
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
 
        }
        public DbSet<Employee> Employees { get; set; }
        //Seeding data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}
