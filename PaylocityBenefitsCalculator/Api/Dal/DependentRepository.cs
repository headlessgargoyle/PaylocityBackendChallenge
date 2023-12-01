using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Dal;

public class DependentRepository : IDependentRepository
{
    private List<Dependent> _dependents;

    public DependentRepository()
    {
        // could inject the DataContext from EntityFramework or the equivalent from any other ORM
        // or set up a DB layer using System.Data

        _dependents = new List<Dependent>
        {
            new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3),
                EmployeeId = 2
            },
            new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23),
                EmployeeId = 2
            },
            new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18),
                EmployeeId = 2
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2),
                EmployeeId = 3
            }
        };
    }

    public Dependent? Get(int id)
    {
        return _dependents.FirstOrDefault(x => x.Id == id);
    }

    // I'm sticking with Lists because that was what was provided
    // but this may be better to be IEnumerable or some other collection type, dependent on provider
    // eg, IQueryable in EF
    public List<Dependent> GetByEmployee(int id)
    {
        return _dependents.Where(x => x.EmployeeId == id).ToList();
    }

    public List<Dependent> GetAll()
    {
        return _dependents;
    }

    public Dependent Create(Dependent dependent)
    {
        if (dependent == null)
            throw new ValidationException("Cannot create null employee");

        if (dependent.Id != 0)
            throw new ValidationException("Updating employees is not supported at this time");

        // potential race condition at scale, could solve with lock assuming no load balancing
        // otherwise this would really be the responsibility of the DB
        dependent.Id = _dependents.Last().Id + 1;
        _dependents.Add(dependent);

        return dependent;
    }
}
