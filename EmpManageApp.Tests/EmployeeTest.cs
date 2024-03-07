using EmpManageApp.Application.Repositories;
using EmpManageApp.Application.Services;
using EmpManageApp.Domain.Entities;
using EmpManageApp.Infrastructure.Repositories;
using Moq;

namespace EmpManageApp.Tests
{
    public class EmployeeTest
    {
        #region Fields
        private Mock<IEmployeeRepository>? mockEmployeeRepository;
        private EmployeeService? employeeService;

        #endregion Fields

        public EmployeeTest()
        {
            this.MockDependencies();
        }

        #region Tests
        [Fact]
        public void GetEmployeeById_ValidEmployee()
        {
            // Arrange
            var employeeService = new EmployeeService(mockEmployeeRepository.Object);
            var expectedEmployee = new Employee { EmployeeID = 1, FirstName = "Lucy" };
            mockEmployeeRepository.Setup(repo => repo.GetEmployeeByIdAsync(1)).Returns(Task.FromResult(expectedEmployee));

            // Act
            var result = employeeService.GetById(1);

            // Assert
            Assert.Equal(expectedEmployee, result);
        }

        [Fact]
        public void GetEmployeeById_InvalidEmployee()
        {
            // Arrange
            var expectedEmployee = new Employee { EmployeeID = 1, FirstName = "Lucy" };
            mockEmployeeRepository.Setup(repo => repo.GetEmployeeByIdAsync(1)).Returns(Task.FromResult(expectedEmployee));

            // Act
            var result = employeeService.GetById(0);

            // Assert
            Assert.Null(result);
        }

        #endregion Tests

        #region Private Methods
        private void MockDependencies()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            employeeService = new EmployeeService(mockEmployeeRepository.Object);

        }
        #endregion Private Methods

    }
}