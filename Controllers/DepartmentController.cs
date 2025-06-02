using EmployeeAppReloaded2.Data;
using EmployeeAppReloaded2.Models;
using EmployeeAppReloaded2.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAppReloaded2.Controllers;

public class DepartmentController : BaseController
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentController(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _departmentRepository.GetAllDepartments();

        var model = result.Select(dep => new DepartmentViewModel
        {
           
            Id = Guid.NewGuid(),
            Name = dep.Name,
            Description = dep.Description
        });

        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDepartmentViewModel model)
    {
        var isDepartmentExist = await _departmentRepository.IsDepartmentExist(model.Name);

        if (isDepartmentExist)
        {
            SetFlashMessage($"Employee with this email {model.Name} already exists.", "error");
            return View(model);
        }

        var department = new Department
        {
            
            Id = Guid.NewGuid(),
            Name = model.Name,
            Description = model.Description
        };

        bool isAdded = await _departmentRepository.AddDepartment(department);

        if (isAdded)
        {
            SetFlashMessage("Department record created successfully!", "success");
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public async Task<IActionResult> Detail(Guid id)
    {
        var result = await _departmentRepository.GetDepartmentById(id);

        if (result is null)
        {
            SetFlashMessage("Department not found.", "error");
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new DepartmentDetailViewModel
        {
             Id = result.Id,
            Name = result.Name,
           Description = result.Description
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _departmentRepository.GetDepartmentById(id);
        var model = new EditDepartmentViewModel
        {
            Id = result.Id,
            Name = result.Name,
           Description = result.Description
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditDepartmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var department = new Department
        {
            Id = model.Id,
            Name = model.Name,
            Description=model.Description
        };

        var isUpdated = await _departmentRepository.EditDepartment(department);

        if (!isEdited)
        {
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _departmentRepository.DeleteDepartment(id);

        if (!result)
        {
            SetFlashMessage("Error deleting department. Please try again.", "error");
            return RedirectToAction(nameof(Index));
        }

        SetFlashMessage("Department deleted successfully.", "success");

        return RedirectToAction(nameof(Index));
    }
}

