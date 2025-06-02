using EmployeeAppReloade2.Data;

namespace EmployeeAppReloade2.Repository;

public interface IEmployeeRepository
{
    Task<bool> AddEmployee(Employee employee);
    Task<List<Employee>> GetAllEmployees();
    Task<Employee> GetEmployeeById(Guid id);
    Task<bool> EditEmployee(Employee employee);
    Task<bool> DeleteEmployee(Guid id);
    Task<bool> IsEmployeeExist(string email);
}
