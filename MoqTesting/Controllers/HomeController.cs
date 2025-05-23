using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MoqTesting.Services;


namespace MoqTesting.Controllers;

public class HomeController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public HomeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    public IActionResult AddEmployee(string name, string department)
    {
        _employeeService.AddEmployee(name, department);
        return Ok($"Employee added: {name} - {department}");
    }
    public IActionResult GetEmployeeById(int id)
    {
        _employeeService.GetEmployeeById(id);
        return Ok($"Employee with ID {id} retrieved.");
    }
    public IActionResult DisplayAll()
    {
        _employeeService.DisplayAll();
        return Ok("Displayed all employees.");
    }

    public IActionResult UpdateEmployee(int id, string newName, string newDepartment)
    {
        _employeeService.UpdateEmployee(id, newName, newDepartment);
        var employee = _employeeService.GetEmployeeById(id);
        return Ok(employee);
    }

    public IActionResult DeleteEmployee(int id)
    {
        _employeeService.DeleteEmployee(id);
        return Ok($"Employee {id} deleted.");
    }


}
