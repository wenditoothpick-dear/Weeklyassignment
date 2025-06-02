using System.ComponentModel.DataAnnotations;

namespace EmployeeAppReloaded2.Models;

public class EditDepartmentViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First name is required!")]
    public string Name { get; set; } = default!;
    public string? Description {get; set;}
}
