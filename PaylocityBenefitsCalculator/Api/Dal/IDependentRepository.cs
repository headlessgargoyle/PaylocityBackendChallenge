using Api.Models;

namespace Api.Dal;

public interface IDependentRepository
{
    Dependent? Get(int id);
    List<Dependent> GetByEmployee(int id);
    List<Dependent> GetAll();
    Dependent Create(Dependent dependent);
}
