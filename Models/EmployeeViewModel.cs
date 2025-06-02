namespace EmployeeAppReloaded2.Models;

public class EmployeeViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Department { get; set; } = default!;

}
public class EmployeesViewModel
{
    public List<EmployeeViewModel> Employees { get; set; } = default!;
}