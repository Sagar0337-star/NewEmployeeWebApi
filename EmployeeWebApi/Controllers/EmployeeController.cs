using AutoMapper;
using Employee.Domain.Employee;
using Employee.Domain.Model;
using EmployeeWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebAPI.Controllers
{
    [Authorize]
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService ;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet("GetAllEmployeeDetails")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EmployeeViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<EmployeeViewModel>>> GetAllEmployeeDetails()
        {
            try
            {
                var employeeList = await _employeeService.GetAllEmployees();

                return Ok(_mapper.Map<List<EmployeeViewModel>>(employeeList));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetEmployeeDetailsById/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeViewModel>> GetEmployeeDetailsById(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest();
                }

                var employeeData = await _employeeService.GetEmployeeById(Id);
                if (employeeData == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<EmployeeViewModel>(employeeData));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddEmployee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeViewModel>> AddEmployee([FromBody] EmployeeViewModel model)
        {
            try
            {
                var employeeData = _mapper.Map<EmployeeModel>(model);
                await _employeeService.AddEmployee(employeeData);

                return Created();

            }
            catch (Exception)
            {
                return BadRequest("Record is not created");
            }
        }

        [HttpPut("UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> UpdateEmployee([FromBody] EmployeeViewModel model)
        {
            try
            {
                var employeeData = _mapper.Map<EmployeeModel>(model);
                var status = await _employeeService.UpdateEmployee(employeeData);
                if (!status)
                {
                    return NotFound("User not found");
                }
                return Ok(status);

            }
            catch (Exception ex)
            {
                return BadRequest("Record is not updated");
            }
        }

        [HttpDelete("DeleteEmployee/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteEmployee(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest();
                }
                var status = await _employeeService.DeleteEmployee(Id);
                if (!status)
                {
                    return NotFound("User not found");
                }
                return Ok(status);
            }
            catch (Exception)
            {

                return BadRequest("Record is not delete");
            }
        }
    }
}
