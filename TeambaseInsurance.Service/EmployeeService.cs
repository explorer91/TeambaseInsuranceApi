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
            var employees = await _repository.EmployeeRepository.GetAllEmployeesAsync();
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Result<IEnumerable<EmployeeDto>>.Success(employeeDtos);
        }

        public async Task<Result<EmployeeDto>> GetEmployeeByIdAsync(int id)
        {
            var employee = await _repository.EmployeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
                return Result<EmployeeDto>.NotFound();

            return Result<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(employee));
        }

        public async Task<Result<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            var employee = _mapper.Map<Employee>(createEmployeeDto);

            _repository.EmployeeRepository.CreateEmployeeAsync(employee);

            await _repository.SaveAsync();

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Result<EmployeeDto>.Success(employeeDto);
        }

        public async Task<Result<bool>> DeleteEmployeeAsync(int id)
        {
            var employee = await _repository.EmployeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
                return Result<bool>.NotFound();

            _repository.EmployeeRepository.DeleteEmployeeAsync(employee);
            await _repository.SaveAsync();

            return Result<bool>.Success(true);
        }
    }
}
