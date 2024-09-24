using Employee.WebApi.Model;

namespace EmployeeWebAPI.Model;

public class EmployeeViewModel
{
    public int EmpId { get; set; }
    public string EmpName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string Gender { get; set; }
    public string Designation { get; set; }
    public DateTime? DOB { get; set; }

    public SkillViewModel Skill { get; set; }
    public List<HobbyViewModel> Hobby { get; set; }
}
