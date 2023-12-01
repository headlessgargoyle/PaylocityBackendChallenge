using Api.Dal;
using System;
using Xunit;

namespace ApiTests.Dal;

[Trait("Category", "Employee")]
[Trait("Category", "Integration")]
public class EmployeeRepositoryTests
{
    // I'm accepting rounding here for test purposes, but realistically we probably still wouldn't want to
    // With actual persistent storage, this could be considerably harder to write as an integration test
    // as what employees exist and what they're paid might not be deterministic

    // This could be handled through mocking however using the IDependentRepository and IEmployeeRepository appropriately
    // Due to the time constraint however I'm preferring an integration test

    [Theory]
    [InlineData(1, 461.54, 2439.27)]
    [InlineData(2, 1363.36, 2189.15)]
    [InlineData(3, 848.62, 4659.50)]
    public void GetPaycheck_ReturnsCorrectValues(int employeeId, decimal expectedDeductions, decimal expectedNet)
    {
        var dependentRepository = new DependentRepository();
        var employeeRepository = new EmployeeRepository(dependentRepository);

        var paycheck = employeeRepository.GetPaycheck(employeeId);

        Assert.Equal(expectedDeductions, Math.Round(paycheck.Deductions, 2));
        Assert.Equal(expectedNet, Math.Round(paycheck.Net, 2));
    }
}
