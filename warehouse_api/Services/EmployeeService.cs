

using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using warehouse_api.Domain;
using warehouse_api.DTOs.Employees;
using warehouse_api.Repositories.Interfaces;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPasswordHasher<Employee> _passwordHasher;

        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IPasswordHasher<Employee> passwordHasher,
             IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<List<EmployeeResponseDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return _mapper.Map<List<EmployeeResponseDto>>(employees);
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int employeeId)
        { 
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if(employee == null)
            {
                return null;
            }

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        public async Task<EmployeeResponseDto> CreateAsync(CreateEmployeeRequestDto requestEmpDto)
        { 
            var employee = _mapper.Map<Employee>(requestEmpDto);

            employee.IsActive = true;

            employee.PasswordHash =
            _passwordHasher.HashPassword(
                employee,
                requestEmpDto.Password);

            var createdEmployee =
                await _employeeRepository.CreateAsync(employee);

            return _mapper.Map<EmployeeResponseDto>(
                createdEmployee);

        }

        public async Task<EmployeeResponseDto?> UpdateAsync(
       int employeeId,
       UpdateEmployeeRequestDto request)
        {
            var employee = await _employeeRepository.GetByIdAsync(
                employeeId);

            if (employee is null)
            {
                return null;
            }

            _mapper.Map(request, employee);

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                employee.PasswordHash =
                    _passwordHasher.HashPassword(
                        employee,
                        request.Password);
            }

            var updatedEmployee =
                await _employeeRepository.UpdateAsync(employee);

            return _mapper.Map<EmployeeResponseDto>(
                updatedEmployee);
        }

        public async Task<bool> DeleteAsync(int employeeId)
        {
            return await _employeeRepository.DeleteAsync(
                employeeId);
        }




    }
}
