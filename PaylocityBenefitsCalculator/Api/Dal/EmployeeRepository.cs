using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Dal;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDependentRepository _dependentRepository;
    private List<Employee> _employees;

    public EmployeeRepository(IDependentRepository dependentRepository)
    {
        // could inject the DataContext from EntityFramework or the equivalent from any other ORM
        // or set up a DB layer using System.Data

        // Including dependentRepository here could be seen as a boundary collision,
        // but it makes sense to do so if downstream consumers (controllers, UI) expect Dependents from an Employee

        _dependentRepository = dependentRepository;

        _employees = new List<Employee>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30),
                Dependents = dependentRepository.GetByEmployee(1)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = dependentRepository.GetByEmployee(2)
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = dependentRepository.GetByEmployee(3)
            }
        };
    }

    public Employee? Get(int id)
    {
        return _employees.FirstOrDefault(x => x.Id == id);
    }

    public List<Employee> GetAll()
    {
        return _employees;
    }

    public Employee Create(Employee employee)
    {
        if (employee == null)
            throw new ValidationException("Cannot create null employee");

        if (employee.Id != 0)
            throw new NotSupportedException("Updating employees is not supported at this time");

        // potential race condition at scale, could solve with lock assuming no load balancing
        // otherwise this would really be the responsibility of the DB
        employee.Id = _employees.Last().Id + 1;

        var validDependents = employee.Dependents.Count(x => x.Relationship == Relationship.Spouse || x.Relationship == Relationship.DomesticPartner);
        if (validDependents > 1)
            throw new ValidationException("Cannot create employee with more than one spouse or domestic partner");

        foreach (var dependent in employee.Dependents)
        {
            dependent.EmployeeId = employee.Id;
            _dependentRepository.Create(dependent);
        }

        _employees.Add(employee);

        return employee;
    }

    // This method could exist in a lot of different places, one approach might be to even put it on the Employee model
    // However, I've opted to put it on the repository because the likelihood is in a production system that this would need
    // to call multiple external integrations, be it DBs or other APIs. Keeping it as part of the DAL therefore makes sense
    public Paycheck? GetPaycheck(int employeeId)
    {
        var employee = Get(employeeId);

        if (employee == null)
            return null;

        var paycheck = new Paycheck();
        paycheck.Gross = employee.Salary / 26;

        var yearlyDeductions = 12000m;
        yearlyDeductions += 600 * 12 * employee.Dependents.Count;
        yearlyDeductions += 200 * 12 * employee.Dependents.Count(x => x.DateOfBirth <= DateTime.Now.AddYears(-50));

        if (employee.Salary > 80000m)
            yearlyDeductions += employee.Salary * 0.02m;

        paycheck.Deductions = yearlyDeductions / 26;

        // rounding the paycheck to a given number of decimal places could make sense,
        // but would likely depend on the law in this area, which I'm not aware of
        // as such, not rounding

        return paycheck;
    }
}
