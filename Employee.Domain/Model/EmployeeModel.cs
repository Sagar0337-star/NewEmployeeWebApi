using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Domain.Model;
public class EmployeeModel
{
    public int EmpId { get; set; }
    public string EmpName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string Gender { get; set; }
    public string Designation { get; set; }
    public DateTime? DOB { get; set; }
    public virtual SkillModel Skill { get; set; }
    public virtual ICollection<HobbyModel> Hobby { get; set; }

}
