using Ardalis.Result;
using Shared.DTOs;

namespace TeambaseInsurance.ServiceContracts
{
    public interface IEmployeeService
    {
        Task<Result<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync();
        Task<Result<EmployeeDto>> GetEmployeeByIdAsync(int id);
        Task<Result<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<Result<bool>> DeleteEmployeeAsync(int id);
    }
}
