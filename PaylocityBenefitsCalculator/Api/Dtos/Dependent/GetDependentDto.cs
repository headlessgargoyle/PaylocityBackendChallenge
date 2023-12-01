using Api.Models;

namespace Api.Dtos.Dependent;

public class GetDependentDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }

    public GetDependentDto() { }

    // There are so many ways one could do mapping,
    // Anything from MemberwiseClone to a third party library like AutoMapper could make sense
    // For ease of development, I'm just going with a mapping constructor
    public GetDependentDto(Models.Dependent? dependent) 
    {
        if (dependent == null)
            return; // could prefer to throw, but then consumers have to handle this more explicitly

        Id = dependent.Id;
        FirstName = dependent.FirstName;
        LastName = dependent.LastName;
        DateOfBirth = dependent.DateOfBirth;
        Relationship = dependent.Relationship;
    }
}
