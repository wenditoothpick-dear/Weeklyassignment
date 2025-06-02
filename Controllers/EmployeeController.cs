using EmployeeAppReloaded2.Data;
using EmployeeAppReloaded2.Models;
using EmployeeAppReloaded2.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAppReloaded2.Controllers;

public class EmployeeController : BaseController
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _employeeRepository.GetAllEmployees();

        var model = result.Select(emp => new EmployeeViewModel
        {
            Id = emp.Id,
            FirstName = emp.FirstName,
            LastName = emp.LastName,
            Email = emp.Email,
            Department = emp.Department
        });

        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeViewModel model)
    {
        var isEmployeeExist = await _employeeRepository.IsEmployeeExist(model.Email);

        if (isEmployeeExist)
        {
            SetFlashMessage($"Employee with this email {model.Email} already exists.", "error");
            return View(model);
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Department = model.Department,
            HireDate = model.HireDate,
            Salary = model.Salary
        };

        bool isAdded = await _employeeRepository.AddEmployee(employee);

        if (isAdded)
        {
            SetFlashMessage("Employee record created successfully!", "success");
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public async Task<IActionResult> Detail(Guid id)
    {
        var result = await _employeeRepository.GetEmployeeById(id);

        if (result is null)
        {
            SetFlashMessage("Employee not found.", "error");
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new EmployeeDetailViewModel
        {
            Id = result.Id,
            FirstName = result.FirstName,
            LastName = result.LastName,
            Email = result.Email,
            Department = result.Department,
            HireDate = result.HireDate.ToShortDateString(),
            Salary = result.Salary.ToString("C")
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _employeeRepository.GetEmployeeById(id);
        var model = new EditEmployeeViewModel
        {
            Id = result.Id,
            FirstName = result.FirstName,
            LastName = result.LastName,
            Email = result.Email,
            Department = result.Department,
            HireDate = result.HireDate,
            Salary = result.Salary
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditEmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var employee = new Employee
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            HireDate = model.HireDate,
            Department = model.Department,
            Salary = model.Salary
        };

        var isUpdated = await _employeeRepository.EditEmployee(employee);

        if (!isUpdated)
        {
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _employeeRepository.DeleteEmployee(id);

        if (!result)
        {
            SetFlashMessage("Error deleting employee. Please try again.", "error");
            return RedirectToAction(nameof(Index));
        }

        SetFlashMessage("Employee deleted successfully.", "success");

        return RedirectToAction(nameof(Index));
    }
}

