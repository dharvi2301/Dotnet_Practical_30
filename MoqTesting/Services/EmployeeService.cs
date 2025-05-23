using MoqTesting.Models;
using MoqTesting.Repository;

namespace MoqTesting.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        // Add a new employee
        public void AddEmployee(string name, string department)
        {
            var employee = new Employee
            {
                Name = name,
                Department = department
            };

            _repository.Add(employee);
            Console.WriteLine($"Employee added: {employee.Name} - {employee.Department}");
        }

        public Employee GetEmployeeById(int id)
        {
            var employee = _repository.GetById(id);
            if (employee != null)
            {
                Console.WriteLine($"Employee found: {employee.Name} - {employee.Department}");
            }
            else
            {
                Console.WriteLine($"Employee with ID {id} not found.");
            }
            return employee;
        }
        // Display all employees
        public List<Employee> DisplayAll()
        {
            var employees = _repository.GetAll();
            Console.WriteLine("Employee List:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Department: {emp.Department}");
            }
            return employees.ToList();
        }

        // Update an employee by ID
        public void UpdateEmployee(int id, string newName, string newDepartment)
        {
            var existing = _repository.GetById(id);
            if (existing != null)
            {
                existing.Name = newName;
                existing.Department = newDepartment;
                _repository.Update(existing);
                Console.WriteLine($"Employee {id} updated.");
            }
            else
            {
                Console.WriteLine($"Employee with ID {id} not found.");
            }
        }

        // Delete an employee by ID
        public void DeleteEmployee(int id)
        {
            var existing = _repository.GetById(id);
            if (existing != null)
            {
                _repository.Delete(id);
                Console.WriteLine($"Employee {id} deleted.");
            }
            else
            {
                Console.WriteLine($"Employee with ID {id} not found.");
            }
        }

        public List<Employee> GetAllEmployee()
        {
            var employees = _repository.GetAll();
            return employees.ToList();
        }
    }
}
