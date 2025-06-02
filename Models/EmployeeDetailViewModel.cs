namespace EmployeeAppReloaded2.Models;

public class EmployeeDetailViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = default!;
    public string Department { get; set; } = default!;
    public string HireDate { get; set; } = default!;
    public string Salary { get; set; } = default!;
}
