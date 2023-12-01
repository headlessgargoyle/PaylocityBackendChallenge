using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public class GetEmployeeDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Salary { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<GetDependentDto> Dependents { get; set; } = new List<GetDependentDto>();


    public GetEmployeeDto() { }

    // There are so many ways one could do mapping,
    // Anything from MemberwiseClone to a third party library like AutoMapper could make sense
    // For ease of development, I'm just going with a mapping constructor
    public GetEmployeeDto(Models.Employee? employee)
    {
        if (employee == null)
            return; // could prefer to throw, but then consumers have to handle this more explicitly

        Id = employee.Id;
        FirstName = employee.FirstName;
        LastName = employee.LastName;
        Salary = employee.Salary;
        DateOfBirth = employee.DateOfBirth;
        Dependents = employee.Dependents.Select(x => new GetDependentDto(x)).ToList();
    }
}
