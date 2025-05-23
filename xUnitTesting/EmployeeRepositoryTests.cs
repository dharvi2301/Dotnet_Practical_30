using Moq;
using MoqTesting.Models;
using MoqTesting.Repository;
using MoqTesting.Services;
using Xunit;

namespace xUnitTesting
{
    public class UnitTest1
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly EmployeeService _employeeService;

        public UnitTest1()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_mockRepo.Object);
        }

        [Fact(DisplayName = "AddEmployee_Should_Call_RepositoryAdd_With_CorrectData")]
        public void AddEmployee_Should_Call_RepositoryAdd_With_CorrectData()
        {
            // Arrange
            var employeeName = "John Doe";
            var employeeDepartment = "IT";

            // Act
            _employeeService.AddEmployee(employeeName, employeeDepartment);

            // Assert
            _mockRepo.Verify(repo => repo.Add(It.Is<Employee>(e => e.Name == "John Doe" && e.Department == "IT")), Times.Once);
        }

        [Fact(DisplayName = "DeleteEmployee_Should_Call_RepositoryDelete_When_EmployeeExists")]
        public void DeleteEmployee_Should_Call_RepositoryDelete_When_EmployeeExists()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee { Id = employeeId, Name = "John Doe", Department = "IT" };
            _mockRepo.Setup(repo => repo.GetById(employeeId)).Returns(employee);

            // Act
            _employeeService.DeleteEmployee(employeeId);

            // Assert
            _mockRepo.Verify(repo => repo.Delete(employeeId), Times.Once);
        }

        [Theory(DisplayName = "DeleteEmployee_Should_NotCall_RepositoryDelete_When_EmployeeNotFound")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void DeleteEmployee_Should_NotCall_RepositoryDelete_When_EmployeeNotFound(int employeeId)
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(employeeId)).Returns((Employee)null);

            // Act
            _employeeService.DeleteEmployee(employeeId);

            // Assert
            _mockRepo.Verify(repo => repo.Delete(employeeId), Times.Never);
        }

        [Fact(DisplayName = "UpdateEmployee_Should_Call_RepositoryUpdate_When_EmployeeExists")]
        public void UpdateEmployee_Should_Call_RepositoryUpdate_When_EmployeeExists()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee { Id = employeeId, Name = "John Doe", Department = "IT" };
            _mockRepo.Setup(repo => repo.GetById(employeeId)).Returns(employee);

            var newName = "Jane Doe";
            var newDepartment = "HR";

            // Act
            _employeeService.UpdateEmployee(employeeId, newName, newDepartment);

            // Assert
            _mockRepo.Verify(repo => repo.Update(It.Is<Employee>(e =>
                e.Id == employeeId && e.Name == newName && e.Department == newDepartment)), Times.Once);
        }

        [Theory(DisplayName = "UpdateEmployee_Should_NotCall_RepositoryUpdate_When_EmployeeNotFound")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void UpdateEmployee_Should_NotCall_RepositoryUpdate_When_EmployeeNotFound(int employeeId)
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(employeeId)).Returns((Employee)null);
            var newName = "Jane Doe";
            var newDepartment = "HR";

            // Act
            _employeeService.UpdateEmployee(employeeId, newName, newDepartment);

            // Assert
            _mockRepo.Verify(repo => repo.Update(It.IsAny<Employee>()), Times.Never);
        }

        [Fact(DisplayName = "DisplayAll_Should_Call_RepositoryGetAll_Once")]
        public void DisplayAll_Should_Call_RepositoryGetAll_Once()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Department = "IT" },
                new Employee { Id = 2, Name = "Jane Smith", Department = "HR" }
            };
            _mockRepo.Setup(repo => repo.GetAll()).Returns(employees);

            // Act
            _employeeService.DisplayAll();

            // Assert
            _mockRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact(DisplayName = "GetEmployeeById_Should_Return_Employee_When_Found")]
        public void GetEmployeeById_Should_Return_Employee_When_Found()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee { Id = employeeId, Name = "John Doe", Department = "IT" };
            _mockRepo.Setup(repo => repo.GetById(employeeId)).Returns(employee);

            // Act
            var result = _employeeService.GetEmployeeById(employeeId);

            // Assert
            _mockRepo.Verify(repo => repo.GetById(employeeId), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact(DisplayName = "GetEmployeeById_Should_Return_Null_When_NotFound")]
        public void GetEmployeeById_Should_Return_Null_When_NotFound()
        {
            // Arrange
            var employeeId = 1;
            _mockRepo.Setup(repo => repo.GetById(employeeId)).Returns((Employee)null);

            // Act
            var result = _employeeService.GetEmployeeById(employeeId);

            // Assert
            _mockRepo.Verify(repo => repo.GetById(employeeId), Times.Once);
            Assert.Null(result);
        }

        [Fact(DisplayName = "GetAllEmployee_Should_Return_All_Employees")]
        public void GetAllEmployee_Should_Return_All_Employees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Department = "IT" },
                new Employee { Id = 2, Name = "Jane Smith", Department = "HR" }
            };
            _mockRepo.Setup(repo => repo.GetAll()).Returns(employees);

            // Act
            var result = _employeeService.GetAllEmployee();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Name == "John Doe" && e.Department == "IT");
            Assert.Contains(result, e => e.Name == "Jane Smith" && e.Department == "HR");
        }
    }
}
