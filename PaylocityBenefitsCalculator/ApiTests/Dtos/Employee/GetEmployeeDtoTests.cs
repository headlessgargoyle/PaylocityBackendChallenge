using Api.Dtos.Employee;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiTests.Dtos.Employee;

[Trait("Category", "Unit")]
[Trait("Category", "GetEmployeeDto")]
public class GetEmployeeDtoTests
{
    [Fact]
    public void Constructor_MapsFromEmployee()
    {
        var employee = new Api.Models.Employee()
        {
            Id = 2,
            FirstName = "Ja",
            LastName = "Morant",
            Salary = 92365.22m,
            DateOfBirth = new DateTime(1999, 8, 10),
            Dependents = new List<Api.Models.Dependent> { new Api.Models.Dependent() }
        };

        var employeeDto = new GetEmployeeDto(employee);

        Assert.Equal(employee.Id, employeeDto.Id);
        Assert.Equal(employee.FirstName, employeeDto.FirstName);
        Assert.Equal(employee.LastName, employeeDto.LastName);
        Assert.Equal(employee.Salary, employeeDto.Salary);
        Assert.Equal(employee.DateOfBirth, employeeDto.DateOfBirth);
        Assert.Equal(employee.Dependents.Count, employeeDto.Dependents.Count);
    }
}
