using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                    new Employee
                    {
                        Id = 1,
                        Name = "Mark",
                        Department = Dept.IT,
                        Email = "mark@pragimtech.com"
                    },
                    new Employee
                    {
                        Id = 2,
                        Name = "Mary",
                        Department = Dept.HR,
                        Email = "mary@pragimtech.com"
                    }
                );
        }
    }
}
