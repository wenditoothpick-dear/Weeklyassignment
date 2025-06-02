using EmployeeAppReloade2.Data;

namespace EmployeeAppReloade2.Repository;

public interface IDepartmentRepository
{
    Task<bool> AddDepartment(Department department);
    Task<List<Department>> GetAllDepartment();
    Task<Employee> GetDepartmentById(Guid id);
    Task<bool> EditDepartment(Department department);
    Task<bool> DeleteDepartment(Guid id);
    Task<bool> IsDepartmentExist(string name);
}
