using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Domain.Model;
public class SkillModel
{
    public int SkillId { get; set; }
    public string SkillName { get; set; }
    public int EmployeeId { get; set; }
    public virtual EmployeeModel Employee { get; set; }
}
