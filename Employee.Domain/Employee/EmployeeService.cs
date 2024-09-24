using Employee.COMMON.Model;
using Employee.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Domain.Employee;
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task AddEmployee(EmployeeModel model)
    {
        await _employeeRepository.AddEmployee(model);
    }

    public async Task<bool> DeleteEmployee(int Id)
    {
        return await _employeeRepository.DeleteEmployee(Id);
    }

    public async Task<string> GenerateToken(UserLogin model, string key, string issuer, string audience)
    {
        return await _employeeRepository.GenerateToken(model, key, issuer, audience);
    }

    public async Task<List<EmployeeModel>> GetAllEmployees()
    {
        return await _employeeRepository.GetAllEmployees();
    }

    public Task<EmployeeModel> GetEmployeeById(int Id)
    {
        return _employeeRepository.GetEmployeeById(Id);
    }

    public Task<bool> UpdateEmployee(EmployeeModel model)
    {
        return _employeeRepository.UpdateEmployee(model);
    }
}
