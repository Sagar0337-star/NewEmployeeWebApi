using Employee.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employee.EF.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeModel>
    {
        public void Configure(EntityTypeBuilder<EmployeeModel> entity)
        {
            entity.ToTable("Employees");

            entity.HasKey(x => x.EmpId);
            entity.Property(x => x.EmpName).HasColumnName("EmployeeName").HasMaxLength(100);
            entity.Property(x => x.Email).HasColumnName("Email").HasMaxLength(150);
            entity.Property(x => x.MobileNumber).HasColumnName("MobileNumber").HasMaxLength(10);
            entity.Property(x => x.Gender).HasColumnName("Gender");
            entity.Property(x => x.Designation).HasColumnName("Designation");
            entity.Property(x => x.DOB).HasColumnName("DOB");
        }
    }
}

