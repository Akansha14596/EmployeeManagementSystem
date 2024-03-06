using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using EmpManageApp.Application.Repositories;
using EmpManageApp.Domain.Entities;

namespace EmpManageApp.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly IDbConnection _connection;
        public EmployeeRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            string sql = @"INSERT INTO Employee (EmployeeID, FirstName, LastName, Email, Phone, DepartmentID, RoleID, IsActive)
                           VALUES (@EmployeeID, @FirstName, @LastName, @Email, @Phone, @DepartmentID, @RoleID, @IsActive);
                           SELECT CAST(SCOPE_IDENTITY() as int)";
            int newId = await _connection.ExecuteScalarAsync<int>(sql, employee);
         //   employee.EmployeeID = newId;
            return employee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            string sql = "DELETE FROM Employee WHERE EmployeeID = @Id";
            var affectedRows = await _connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            string sql = "SELECT * FROM Employee";
            return await _connection.QueryAsync<Employee>(sql);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            string sql = "SELECT * FROM Employee WHERE EmployeeID = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id });
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            string sql = @"UPDATE Employee SET FirstName = @FirstName, LastName = @LastName,
                           Email = @Email, Phone = @Phone, DepartmentID = @DepartmentID,
                           RoleID = @RoleID, IsActive = @IsActive
                           WHERE EmployeeID = @EmployeeId";
            employee.EmployeeID = id;
            var affectedRows = await _connection.ExecuteAsync(sql, employee);
            if (affectedRows > 0)
                return employee;
            else
                return null;
        }
    }
}
