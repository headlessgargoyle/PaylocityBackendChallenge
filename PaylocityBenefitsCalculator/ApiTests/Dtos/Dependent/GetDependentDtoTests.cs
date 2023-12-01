using Api.Dtos.Dependent;
using Api.Models;
using System;
using Xunit;

namespace ApiTests.Dtos.Dependent;

[Trait("Category", "Unit")]
[Trait("Category", "GetDependentDto")]
public class GetDependentDtoTests
{
    [Fact]
    public void Constructor_MapsFromDependent()
    {
        var dependent = new Api.Models.Dependent()
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3),
            EmployeeId = 2
        };

        var dependentDto = new GetDependentDto(dependent);

        Assert.Equal(dependent.Id, dependentDto.Id);
        Assert.Equal(dependent.FirstName, dependentDto.FirstName);
        Assert.Equal(dependent.LastName, dependentDto.LastName);
        Assert.Equal(dependent.DateOfBirth, dependentDto.DateOfBirth);
        Assert.Equal(dependent.Relationship, dependentDto.Relationship);
    }
}
