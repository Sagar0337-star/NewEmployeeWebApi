using Employee.COMMON.Model;
using Employee.Domain.Employee;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IConfiguration _configuration;

    public LoginController(IEmployeeService employeeService, IConfiguration configuration)
    {
        _employeeService = employeeService;
        _configuration = configuration;
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
    {
        try
        {

            var key = _configuration.GetValue<string>("JwtSettings:SecretKey");
            var issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
            var audience = _configuration.GetValue<string>("JwtSettings:Audience");

            var token = await _employeeService.GenerateToken(userLogin, key, issuer, audience);
            if (token == "")
            {
                return NotFound();
            }
            return Ok(new TokenResponse
            {
                Token = token
            });
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
