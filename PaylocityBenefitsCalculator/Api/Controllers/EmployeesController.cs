using Api.Dal;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    // API operations are currently synchronous, but that could easily change dependent on DB provider.

    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository) 
    {
        _employeeRepository = employeeRepository;
    }

    // could, probably even should, use the GetEmployeeDto here (albeit, renamed), but for ease of mapping just reusing Employee
    [SwaggerOperation(Summary = "Create employee with dependents")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Create([FromBody] Employee employee)
    {
        try
        {
            var created = _employeeRepository.Create(employee);

            var result = new ApiResponse<Employee>
            {
                Data = created,
                Success = true
            };

            return Created($"/employees/{created.Id}", result); // or Ok()
        } 
        catch (Exception ex)
        {
            var result = new ApiResponse<Employee>
            {
                Data = employee,
                Success = false,
                Message = ex.Message,
                Error = ex.ToString()
            };
            return StatusCode(500, result);
        }
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(404, "No employee with that ID found")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = _employeeRepository.Get(id);

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = new GetEmployeeDto(employee),
            Success = employee != null
        };

        if (result.Success)
            return Ok(result);

        result.Data = null;
        result.Error = "NotFound";
        return NotFound(result);
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<IEnumerable<GetEmployeeDto>>>> GetAll()
    {
        var employees = _employeeRepository.GetAll().Select(x => new GetEmployeeDto(x));

        var result = new ApiResponse<IEnumerable<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };

        return Ok(result);
    }

    [SwaggerOperation(Summary = "Get paycheck for given employee ID")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(404, "No employee with that ID found")]
    [HttpGet("paycheck/{id:int}")]
    public async Task<ActionResult<ApiResponse<Paycheck>>> GetPaycheck(int id)
    {
        var paycheck = _employeeRepository.GetPaycheck(id);

        var result = new ApiResponse<Paycheck>
        {
            Data = paycheck,
            Success = paycheck != null
        };

        if (result.Success)
            return Ok(result);

        result.Data = null;
        result.Error = "NotFound";
        return NotFound(result);
    }
}
