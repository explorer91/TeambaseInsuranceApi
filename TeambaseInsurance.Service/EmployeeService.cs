using Ardalis.Result;
using AutoMapper;
using Entities.Models;
using Shared.DTOs;
using TeambaseInsurance.RepositoryContracts;
using TeambaseInsurance.ServiceContracts;

namespace TeambaseInsurance.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync()
        {
            try
            {
                var employees = await _repository.EmployeeRepository.GetAllEmployeesAsync();
                var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
                return Result<IEnumerable<EmployeeDto>>.Success(employeeDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<EmployeeDto>>.Error(ex.Message);
            }
        }

        public async Task<Result<EmployeeDto>> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var employee = await _repository.EmployeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return Result<EmployeeDto>.NotFound();
                    
                return Result<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(employee));
            }
            catch (Exception ex)
            {
                return Result<EmployeeDto>.Error(ex.Message);
            }
        }

        public async Task<Result<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                var employee = _mapper.Map<Employee>(createEmployeeDto);
                
                _repository.EmployeeRepository.CreateEmployeeAsync(employee);
                
                await _repository.SaveAsync();

                var employeeDto = _mapper.Map<EmployeeDto>(employee);

                return Result<EmployeeDto>.Success(employeeDto);
            }
            catch (Exception ex)
            {
                return Result<EmployeeDto>.Error(ex.Message);
            }
        }

        public async Task<Result<EmployeeDto>> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                var existingEmployee = await _repository.EmployeeRepository.GetEmployeeByIdAsync(id);
                if (existingEmployee == null)
                    return Result<EmployeeDto>.NotFound();

                // Map only the properties that are provided
                _mapper.Map(updateEmployeeDto, existingEmployee);
                
                // Update timestamp
                existingEmployee.LastModified = DateTime.UtcNow;

                // Update the employee
                _repository.EmployeeRepository.UpdateEmployeeAsync(existingEmployee);
                await _repository.SaveAsync();

                // Map the updated entity to DTO
                var employeeDto = _mapper.Map<EmployeeDto>(existingEmployee);

                return Result<EmployeeDto>.Success(employeeDto);
            }
            catch (Exception ex)
            {
                return Result<EmployeeDto>.Error(ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _repository.EmployeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return Result<bool>.NotFound();

                _repository.EmployeeRepository.DeleteEmployeeAsync(employee);
                await _repository.SaveAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Error(ex.Message);
            }
        }
    }
}
