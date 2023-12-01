using Api.Dal;
using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private IDependentRepository _dependentRepository;

    public DependentsController(IDependentRepository dependentRepository)
    {
        _dependentRepository = dependentRepository;
    }

    // Dependent creation skipped as it's handled as part of EmployeeCreation

    // API operations are currently synchronous, but that could easily change dependent on DB provider.

    [SwaggerOperation(Summary = "Get dependent by id")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(404, "No dependent with that ID found")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = _dependentRepository.Get(id);

        var result = new ApiResponse<GetDependentDto>
        {
            Data = new GetDependentDto(dependent),
            Success = dependent != null
        };

        if (result.Success)
            return Ok(result);

        result.Data = null;
        result.Error = "NotFound";
        return NotFound(result);
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<IEnumerable<GetDependentDto>>>> GetAll()
    {
        var employees = _dependentRepository.GetAll().Select(x => new GetDependentDto(x));

        var result = new ApiResponse<IEnumerable<GetDependentDto>>
        {
            Data = employees,
            Success = true
        };

        return Ok(result);
    }
}
