using Employee.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.EF.Configurations
{
    public class HobbyConfiguration : IEntityTypeConfiguration<HobbyModel>
    {
        public void Configure(EntityTypeBuilder<HobbyModel> entity)
        {
            entity.ToTable("Hobbies");

            entity.HasKey(x => x.HobbyId);
            entity.Property(x => x.HobbyName).HasColumnName("HobbyName");

            entity.HasOne(x => x.Employee)
                .WithMany(x => x.Hobby)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}