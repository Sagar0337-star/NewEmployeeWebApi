using Employee.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.EF.Configurations;
public class SkillConfiguration : IEntityTypeConfiguration<SkillModel>
{
    public void Configure(EntityTypeBuilder<SkillModel> entity)
    {
        entity.ToTable("Skills");

        entity.HasKey(x => x.SkillId);
        entity.Property(x => x.SkillName).HasColumnName("SkillName");

        entity.HasOne(x => x.Employee)
            .WithOne(x => x.Skill)
            .HasForeignKey<SkillModel>(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
