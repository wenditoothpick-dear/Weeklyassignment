namespace EmployeeAppReloaded2.Data;

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
   public string? Description {get; set;}
   public ICollection<Employee> Employees { get; set; } = [];
}
