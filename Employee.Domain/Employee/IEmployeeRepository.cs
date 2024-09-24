using Employee.COMMON.Model;
using Employee.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Domain.Employee;
public interface IEmployeeRepository 
{
    Task<string> GenerateToken(UserLogin model, string key, string issuer, string audience);
    Task<List<EmployeeModel>> GetAllEmployees();
    Task<EmployeeModel> GetEmployeeById(int Id);
    Task AddEmployee(EmployeeModel model);
    Task<bool> UpdateEmployee(EmployeeModel model);
    Task<bool> DeleteEmployee(int Id);
}
