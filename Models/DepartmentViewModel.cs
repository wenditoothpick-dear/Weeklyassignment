namespace EmployeeAppReloaded2.Models;

public class DepartmentViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Department { get; set; } = default!;

}
public class DepartmentsViewModel
{
    public List<DepartmentViewModel> Departments { get; set; } = default!;
}