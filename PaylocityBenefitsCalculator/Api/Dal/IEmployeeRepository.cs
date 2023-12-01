using Api.Models;

namespace Api.Dal;

public interface IEmployeeRepository
{
    Employee? Get(int id);
    List<Employee> GetAll();
    Employee Create(Employee employee);
    Paycheck? GetPaycheck(int employeeId);
}
