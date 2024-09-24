using Employee.COMMON.Model;
using Employee.Domain.Employee;
using Employee.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Employee.EF.Repositories;
public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDBContext _employeeDB;
    public EmployeeRepository(EmployeeDBContext employeeDB)
    {
        _employeeDB = employeeDB;
    }

    public async Task AddEmployee(EmployeeModel model)
    {
        await _employeeDB.Employees.AddAsync(model);
        await _employeeDB.SaveChangesAsync();
    }

    public async Task<bool> DeleteEmployee(int Id)
    {
        var data = await _employeeDB.Employees.FirstOrDefaultAsync(x => x.EmpId == Id);
        if (data != null)
        {
            _employeeDB.Employees.Remove(data);
            await _employeeDB.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<string> GenerateToken(UserLogin model, string key, string issuer, string audience)
    {
        var user = await _employeeDB.Employees.AnyAsync(x => x.EmpName.ToLower() == model.EmpName.ToLower() && x.Email.ToLower() == model.Email.ToLower());
        if (user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.EmpName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, model.EmpName),
                new Claim(ClaimTypes.Email, model.Email)
            };

            var tokenOptions = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: signInCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        return string.Empty;
    }

    public async Task<List<EmployeeModel>> GetAllEmployees()
    {
        var employeeList = await _employeeDB.Employees
                                                             .Include(x => x.Skill)
                                                             .Include(x => x.Hobby)
                                                             .ToListAsync();
        return employeeList;
    }

    public async Task<EmployeeModel> GetEmployeeById(int Id)
    {
        var employeeData = await _employeeDB.Employees
                                                       .Include(x => x.Skill)
                                                       .Include(x => x.Hobby)
                                                       .Where(x => x.EmpId == Id)
                                                       .FirstOrDefaultAsync();

        return employeeData;
    }

    public async Task<bool> UpdateEmployee(EmployeeModel model)
    {
        bool isUserExists = true;
        var employeeData = await _employeeDB.Employees
                            .Include(x => x.Skill)
                            .Include(x => x.Hobby)
                            .Where(x => x.EmpId == model.EmpId)
                            .FirstOrDefaultAsync();

        if (employeeData != null)
        {
            employeeData.EmpName = model.EmpName;
            employeeData.Email = model.Email;
            employeeData.Gender = model.Gender;
            employeeData.Designation = model.Designation;
            employeeData.DOB = model.DOB;

            if (await _employeeDB.Employees.AnyAsync(x => x.EmpId == model.Skill.EmployeeId))
            {
                employeeData.Skill.SkillName = model.Skill.SkillName;
            }
            else
            {
                return false;
            }

            foreach (var item in model.Hobby)
            {
                isUserExists = await _employeeDB.Employees.AnyAsync(x => x.EmpId == item.EmployeeId);
            }

            if (!isUserExists)
            {
                return false;
            }
            else
            {

                var newHobbies = model.Hobby.Select(x => new HobbyModel
                {
                    HobbyId = x.HobbyId,
                    HobbyName = x.HobbyName,
                    EmployeeId = x.EmployeeId
                }).ToList();

                employeeData.Hobby = newHobbies;
            }

            _employeeDB.Entry(employeeData).State = EntityState.Modified;
            await _employeeDB.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }
}
