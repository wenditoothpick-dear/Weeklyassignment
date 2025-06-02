using System.ComponentModel.DataAnnotations;

namespace EmployeeAppReloaded2.Models;

public class EditEmployeeViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First name is required!")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "Last name is required!")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Department name is required!")]
    public string Department { get; set; } = default!;

    [Required(ErrorMessage = "Please select a valid hired date!")]
    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "Salary is required!")]
    public decimal Salary { get; set; }
}
