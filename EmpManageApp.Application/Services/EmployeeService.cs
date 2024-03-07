using EmpManageApp.Application.Repositories;
using EmpManageApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManageApp.Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public Employee GetById(int id)
        {
            return _employeeRepository.GetEmployeeByIdAsync(id).Result;
        }

    }
}
