using Employee.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Employee.EF;
public class EmployeeDBContext : DbContext
{
    public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options)
    {

    }

    public DbSet<EmployeeModel> Employees { get; set; }
    public DbSet<SkillModel> Skills { get; set; }
    public DbSet<HobbyModel> Hobbies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetAssembly(typeof(EmployeeDBContext)));

    }
}
